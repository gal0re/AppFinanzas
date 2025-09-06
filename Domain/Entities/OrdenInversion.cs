namespace AppFinanzas.Domain.Entities
{
    public class OrdenInversion
    {
        public int Id { get; set; }

        public int IdCuenta { get; set; }
        
        public int IdActivo { get; set; }

        public string NombreActivo { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public char Operacion { get; set; }

        public int EstadoId { get; set; }

        public decimal MontoTotal { get; set; }


        // Entidades relacionadas
        public Activo Activo { get; set; }
        public EstadoOrden Estado { get; set; }


    }
}
