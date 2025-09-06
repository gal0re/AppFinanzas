using AppFinanzas.Application.Interfaces;
using AppFinanzas.Domain.Entities;

namespace AppFinanzas.Application.Strategies
{
    public class StockPricingStrategy : IOrderPricingStrategy
    {
        public decimal CalcularTotal(OrdenInversion orden, Activo activo)
        {
            // Base = PrecioUnitario * Cantidad
            var baseAmount = activo.PrecioUnitario * orden.Cantidad;

            // Comisión 0,6%
            var commission = baseAmount * 0.006m;

            // IVA 21% sobre comisión
            var tax = commission * 0.21m;

            var total = baseAmount + commission + tax;

            return RoundMoney(total);
        }

        private static decimal RoundMoney(decimal v)
            => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}