using AppFinanzas.Application.Interfaces;
using AppFinanzas.Domain.Entities;

namespace AppFinanzas.Application.Strategies
{
    public class FundPricingStrategy : IOrderPricingStrategy
    {
        public decimal CalcularTotal(OrdenInversion orden, Activo activo)
        {
            // Sin comisiones ni impuestos
            var total = orden.Precio * orden.Cantidad;

            return RoundMoney(total);
        }

        private static decimal RoundMoney(decimal v)
            => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}