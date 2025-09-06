namespace AppFinanzas.Domain.Entities
{
    public class EstadoOrden
    {
        public int Id { get; set; }

        public string DescripcionEstado{ get; set; }

        // Entidades relacionadas
        public ICollection<OrdenInversion> Ordenes { get; set; }

    }
}
