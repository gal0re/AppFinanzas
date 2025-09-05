namespace AppFinanzas.Application.Strategies
{
    public class PricingStrategyFactory
    {
        public IOrderPricingStrategy Resolve(int tipoActivoId) => tipoActivoId switch
        {
            1 => new StockPricingStrategy(), // Acción
            2 => new BondPricingStrategy(),  // Bono
            3 => new FundPricingStrategy(),  // FCI
            _ => throw new NotSupportedException($"TipoActivo {tipoActivoId} no soportado")
        };
    }

}
