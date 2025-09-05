namespace AppFinanzas.Application.DTOs
{
    public class UpdateOrderStatusRequest
    {
        public int EstadoId { get; set; } // 0=En proceso, 1=Ejecutada, 3=Cancelada
    }

}
