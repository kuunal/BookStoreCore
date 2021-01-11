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

        /// <summary>
        /// Inserts the specified book in wishlist.
        /// </summary>
        /// <param name="wishlist">The wishlist.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="BookstoreException">
        /// Invalid user id
        /// or
        /// Invalid book id
        /// or
        /// Already in wishlist
        /// </exception>
        public async Task<WishlistResponseDto> Insert(WishlistDto wishlist, int userId)
        {
            try
            {
                return await _repository.Insert(wishlist, userId);
            } catch (SqlException e) when (e.Message.Contains("FK__wishlist__userId"))
            {
                throw new BookstoreException("Invalid user id");
            }
            catch (SqlException e) when (e.Message.Contains("FK__wishlist__bookId"))
            {
                throw new BookstoreException("Invalid book id");
            }catch(SqlException e) when(e.Number == SqlErrorNumbers.DUPLICATEKEY)
            {
                throw new BookstoreException("Already in wishlist");
            }
        }

        /// <summary>
        /// Gets the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Wishlist items in WishlistDto object</returns>
        public Task<List<WishlistResponseDto>> Get(int userId)
        {
            return _repository.Get(userId);
        }

        /// <summary>
        /// Deletes the specified book identifier.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Boolean result in 1/0 format</returns>
        public async Task<int> Delete(int bookId, int userId)
        {
            return await _repository.Delete(bookId, userId);
        }
    }
}
