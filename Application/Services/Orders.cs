using AppFinanzas.Application.DTOs;
using AppFinanzas.Application.Interfaces;
using AppFinanzas.Application.Strategies;
using AppFinanzas.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using AppFinanzas.Infrastructure.Persistence;

namespace AppFinanzas.Application.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly FinanzasContext _db;
        private readonly PricingStrategyFactory _factory;

        public OrdersService(FinanzasContext db, PricingStrategyFactory factory)
        {
            _db = db;
            _factory = factory;
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            // Guard Clauses rápidos
            if (request.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a 0.");
            if (request.Operacion != 'C' && request.Operacion != 'V')
                throw new ArgumentException("La operación debe ser 'C' o 'V'.");

            // Cargar activo
            var activo = await _db.Activos
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.ActivoId);

            if (activo is null)
                throw new KeyNotFoundException($"El activo {request.ActivoId} no existe.");

            // 2) Precio efectivo según tipo de activo
            // 1=Acción, 2=Bono, 3=FCI (ajustá si tus IDs son otros)
            decimal precioEfectivo;
            switch (activo.TipoActivoId)
            {
                case 1: // Acción
                    if (request.Precio.HasValue)
                        throw new ArgumentException("Para ACCIONES el precio lo aporta la BBDD; no envíes 'Precio'.");
                    if (activo.PrecioUnitario <= 0)
                        throw new InvalidOperationException("El activo (Acción) no tiene precio unitario válido.");
                    precioEfectivo = activo.PrecioUnitario;
                    break;

                case 2: // Bono
                case 3: // FCI
                    if (!request.Precio.HasValue || request.Precio.Value <= 0)
                        throw new ArgumentException("Para BONO/FCI debés enviar 'Precio' > 0 en el request.");
                    precioEfectivo = request.Precio.Value;
                    break;

                default:
                    throw new NotSupportedException($"Tipo de activo {activo.TipoActivoId} no soportado.");
            }

            // 3) Strategy por tipo de activo
            var strategy = _factory.Resolve(activo.TipoActivoId);

            // 4) Construir entidad de dominio
            var orden = new OrdenInversion
            {
                IdCuenta = request.CuentaId,
                IdActivo = activo.Id,
                NombreActivo = activo.Nombre,
                Cantidad = request.Cantidad,
                Operacion = request.Operacion,
                Precio = precioEfectivo,
                EstadoId = 0
            };

            // 5) Calcular total y redondear
            var total = strategy.CalcularTotal(orden, activo);
            orden.MontoTotal = RoundMoney(total);

            // 6) Persistir
            _db.Ordenes.Add(orden);
            await _db.SaveChangesAsync();

            // 7) Mapear response
            return new OrderResponse
            {
                Id = orden.Id,
                NombreActivo = orden.NombreActivo,
                Operacion = orden.Operacion,
                Cantidad = orden.Cantidad,
                PrecioUsado = orden.Precio,
                MontoTotal = orden.MontoTotal,
                EstadoId = orden.EstadoId
            };
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            var orden = await _db.Ordenes
                .AsNoTracking()
                .Include(o => o.Activo)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) return null;

            return new OrderResponse
            {
                Id = orden.Id,
                NombreActivo = orden.NombreActivo,
                Operacion = orden.Operacion,
                Cantidad = orden.Cantidad,
                PrecioUsado = orden.Precio,
                MontoTotal = orden.MontoTotal,
                EstadoId = orden.EstadoId
            };
        }

        public async Task UpdateStatusAsync(int id, int nuevoEstadoId)
        {
            var orden = await _db.Ordenes.FirstOrDefaultAsync(o => o.Id == id);
            if (orden is null)
                throw new KeyNotFoundException($"La orden {id} no existe.");

            if (!ValidStatus(nuevoEstadoId))
                throw new ArgumentOutOfRangeException(nameof(nuevoEstadoId), "Estado inválido. Valores permitidos: 0, 1, 3.");

            if (orden.EstadoId == nuevoEstadoId)
                return;

            if (!CanTransition(orden.EstadoId, nuevoEstadoId))
                throw new InvalidOperationException($"Transición no permitida: {orden.EstadoId} → {nuevoEstadoId}.");

            orden.EstadoId = nuevoEstadoId;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orden = await _db.Ordenes.FirstOrDefaultAsync(o => o.Id == id);
            if (orden is null)
                throw new KeyNotFoundException($"La orden {id} no existe.");

            if (orden.EstadoId == 1)
                throw new InvalidOperationException("No se puede eliminar una orden ejecutada.");

            _db.Ordenes.Remove(orden);
            await _db.SaveChangesAsync();
        }

        //  Helpers
        private static bool ValidStatus(int estadoId) => estadoId is 0 or 1 or 3;

        private static bool CanTransition(int from, int to)
            => from == 0 && (to == 1 || to == 3);

        private static decimal RoundMoney(decimal v)
            => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}
