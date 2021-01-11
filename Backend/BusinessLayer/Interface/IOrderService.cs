using ModelLayer.OrderDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IOrderService
    {
        Task<OrderResponseDto> Add(OrderRequestDto orderRequest, int userId);
    }
}
