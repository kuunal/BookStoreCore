using ModelLayer.CartDto;
using ModelLayer.OrderDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICartService
    {
        Task<CartResponseDto> Insert(CartRequestDto cart, int userId);
        Task<CartDetailedResponseDto> Get(int userId);
        Task<CartResponseDto> Update(CartRequestDto cart, int userId);
        Task<int> Delete(int bookId, int userId);
        Task<List<OrderResponseDto>> PlaceOrder(int userId, int addressId);

    }
}
