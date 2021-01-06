using ModelLayer.CartDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICartRepository
    {
        Task<CartResponseDto> Insert(CartRequestDto cart, int userId);
        Task<int> Delete(int bookId, int userId);
        Task<CartResponseDto> Update(CartRequestDto cart, int userId);
        Task<List<CartResponseDto>> Get(int userId);

    }
}
