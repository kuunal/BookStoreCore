using ModelLayer.CartDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICartRepository
    {
        Task<CartResponseDto> Insert(CartRequestDto cart);
        Task<int> Delete(CartRequestDto wishlist);
        Task<CartResponseDto> Update(CartRequestDto wishlist);
        Task<List<CartResponseDto>> Get();

    }
}
