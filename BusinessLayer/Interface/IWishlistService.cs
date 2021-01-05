using ModelLayer.WishlistDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IWishlistService
    {
        Task<WishlistDto> Insert(WishlistDto wishlist);
        Task<List<WishlistDto>> Get();
        Task<int> Delete(WishlistDto wishlist);

    }
}
