using AppFinanzas.Application.DTOs;
using AppFinanzas.Application.Interfaces;
using AppFinanzas.Application.Strategies;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            // 1) Cargar activo
            var activo = await _db.Activos.FirstOrDefaultAsync();

            if (activo == null)
                throw new KeyNotFoundException("No se encontró el activo.");
            // 2) Determinar estrategia por TipoActivoId

            // 3) Validaciones de negocio (precio requerido/prohibido, etc.)
            // 4) Construir OrdenInversion (set NombreActivo, EstadoId=0)
            // 5) Calcular MontoTotal usando strategy
            // 6) Guardar y mapear a OrderResponse (incluí PrecioUsado)
            throw new NotImplementedException();
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            // TODO: incluir Activo si lo necesitás para response
            throw new NotImplementedException();
        }

        public async Task UpdateStatusAsync(int id, int nuevoEstadoId)
        {
            // 1) Traer orden
            // 2) Validar transición (En proceso -> Ejecutada/Cancelada)
            // 3) Guardar
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            // TODO: eliminar o soft-delete (como prefieras para el challenge)
            throw new NotImplementedException();
        }
    }
}
