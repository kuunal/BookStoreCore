using ModelLayer.WishlistDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IWishlistService
    {
        Task<WishlistResponseDto> Insert(WishlistDto wishlist, int userId);
        Task<List<WishlistResponseDto>> Get(int userId);
        Task<int> Delete(int bookId, int userId);

    }
}
