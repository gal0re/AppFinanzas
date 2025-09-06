using AppFinanzas.Application.Interfaces;

namespace AppFinanzas.Application.Strategies
{
    public class PricingStrategyFactory
    {
        public IOrderPricingStrategy Resolve(int tipoActivoId) => tipoActivoId switch
        {
            1 => new StockPricingStrategy(),
            2 => new BondPricingStrategy(),
            3 => new FundPricingStrategy(),
            _ => throw new NotSupportedException($"TipoActivo {tipoActivoId} no soportado")
        };
    }
}
