namespace AppFinanzas.Application.DTOs
{
    public class CreateOrderRequest
    {
        public int CuentaId { get; set; }
        public int ActivoId { get; set; }
        public char Operacion { get; set; }
        public int Cantidad { get; set; }
        public decimal? Precio { get; set; }
    }

}
