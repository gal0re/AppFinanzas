namespace AppFinanzas.Application.DTOs
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string NombreActivo { get; set; } = string.Empty;
        public char Operacion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUsado { get; set; }
        public decimal MontoTotal { get; set; }
        public int EstadoId { get; set; }
    }

}
