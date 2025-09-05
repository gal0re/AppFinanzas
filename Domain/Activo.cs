namespace AppFinanzas.Models
{
    public class Activo
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int TipoActivoId { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Entidades relacionadas
        public TipoActivo TipoActivo { get; set; }
        public ICollection<OrdenInversion> Ordenes { get; set; }
    }
}
