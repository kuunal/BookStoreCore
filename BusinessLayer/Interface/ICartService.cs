using ModelLayer.CartDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICartService
    {
        Task<CartResponseDto> Insert(CartRequestDto cart, int userId);
        Task<List<CartResponseDto>> Get(int userId);
        Task<CartResponseDto> Update(CartRequestDto cart, int userId);
        Task<int> Delete(CartRequestDto cart, int userId);


    }
}
