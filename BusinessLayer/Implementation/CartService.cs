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

        public async Task<CartResponseDto> Insert(CartRequestDto cart)
        {
            try
            {
                return await _repository.Insert(cart);
            }catch(SqlException e) when(e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid user id!");
            }
            catch (SqlException e) when (e.Number == SqlErrorNumbers.DUPLICATEKEY)
            {
                throw new BookstoreException("Already in cart");
            }
        }
    }
}
