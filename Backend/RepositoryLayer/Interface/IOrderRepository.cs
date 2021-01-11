using ModelLayer.OrderDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IOrderRepository
    {
        Task<OrderResponseDto> Add(OrderRequestDto order, int userId);
    }
}
