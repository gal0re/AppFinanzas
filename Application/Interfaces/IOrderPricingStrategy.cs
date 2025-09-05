using AppFinanzas.Models;

namespace AppFinanzas.Application.Interfaces
{
    public interface IOrderPricingStrategy
    {
        decimal CalcularTotal(OrdenInversion orden, Activo activo);
    }
}
