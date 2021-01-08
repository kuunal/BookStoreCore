using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using ModelLayer.CartDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Inserts the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Added item info</returns>
        /// <exception cref="BookstoreException">
        /// Invalid user id!
        /// or
        /// Already in cart
        /// </exception>
        public async Task<CartResponseDto> Insert(CartRequestDto cart, int userId)
        {
            try
            {
                return await _repository.Insert(cart, userId);
            }catch(SqlException e) when(e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid user id!");
            }
            catch (SqlException e) when (e.Number == SqlErrorNumbers.DUPLICATEKEY)
            {
                throw new BookstoreException("Already in cart");
            }
        }

        /// <summary>
        /// Deletes the specified book identifier from  cart.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Boolean result in 1/0 format</returns>
        public async Task<int> Delete(int bookId, int userId)
        {
            return await _repository.Delete(bookId, userId);
        }

        /// <summary>
        /// Updates the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Updated cart item infomartion</returns>
        public async Task<CartResponseDto> Update(CartRequestDto cart, int userId)
        {
            return await _repository.Update(cart, userId);
        }

        /// <summary>
        /// Gets the cart using specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<CartResponseDto>> Get(int userId)
        {
            return await _repository.Get(userId);
        }
    }
}
