using ModelLayer.CartDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICartService
    {
        Task<CartResponseDto> Insert(CartRequestDto cart);
        Task<List<CartResponseDto>> Get();
        Task<CartResponseDto> Update(CartRequestDto cart);
        Task<int> Delete(CartRequestDto cart);


    }
}
