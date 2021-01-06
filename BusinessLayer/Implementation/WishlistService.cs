using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using ModelLayer.WishlistDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _repository;

        public WishlistService(IWishlistRepository repository)
        {
            _repository = repository;
        }

        public async Task<WishlistDto> Insert(WishlistDto wishlist, int userId)
        {
            try
            {
                if (await _repository.Insert(wishlist, userId) == 1)
                {
                    return wishlist;
                }
                return null;
            } catch (SqlException e) when (e.Message.Contains("FK__wishlist__userId"))
            {
                throw new BookstoreException("Invalid user id");
            }
            catch (SqlException e) when (e.Message.Contains("FK__wishlist__bookId"))
            {
                throw new BookstoreException("Invalid book id");
            }catch(SqlException e) when(e.Message.Contains("PK__wishlist__bookId__userId"))
            {
                throw new BookstoreException("Already in wishlist");
            }
        }
        public Task<List<WishlistDto>> Get(int userId)
        {
            return _repository.Get(userId);
        }

        public async Task<int> Delete(int bookId, int userId)
        {
            return await _repository.Delete(bookId, userId);
        }
    }
}
