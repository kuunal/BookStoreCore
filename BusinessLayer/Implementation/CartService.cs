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

        public async Task<int> Delete(CartRequestDto cart, int userId)
        {
            return await _repository.Delete(cart, userId);
        }

        public async Task<CartResponseDto> Update(CartRequestDto cart, int userId)
        {
            return await _repository.Update(cart, userId);
        }
        public async Task<List<CartResponseDto>> Get(int userId)
        {
            return await _repository.Get(userId);
        }
    }
}
