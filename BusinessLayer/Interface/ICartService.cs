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

    }
}
