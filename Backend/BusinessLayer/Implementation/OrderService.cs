using BusinessLayer.Interface;
using ModelLayer.OrderDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderResponseDto> Add(OrderRequestDto orderRequest, int userId)
        {
            return await _repository.Add(orderRequest, int userId);
        }
    }
}
