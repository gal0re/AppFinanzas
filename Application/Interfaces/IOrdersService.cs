using AppFinanzas.Application.DTOs;

namespace AppFinanzas.Application.Interfaces
{
    public interface IOrdersService
    {
        Task<OrderResponse> CreateAsync(CreateOrderRequest request);
        Task<OrderResponse?> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, int nuevoEstadoId);
        Task DeleteAsync(int id);
    }

}
