using AppFinanzas.Application.Interfaces;
using AppFinanzas.Domain.Entities;

namespace AppFinanzas.Application.Strategies
{
    public class BondPricingStrategy : IOrderPricingStrategy
    {
        public decimal CalcularTotal(OrdenInversion orden, Activo activo)
        {
            // Base = Precio (enviado en la orden) * Cantidad
            var baseAmount = orden.Precio * orden.Cantidad;

            // Comisión 0,2%
            var commission = baseAmount * 0.002m;

            // IVA 21% sobre comisión
            var tax = commission * 0.21m;

            var total = baseAmount + commission + tax;

            return RoundMoney(total);
        }

        private static decimal RoundMoney(decimal v)
            => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}