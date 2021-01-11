using ModelLayer.CartDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class CartRepository : ICartRepository
    {
        private readonly IDBContext _dbContext;
        private readonly IBooksRepository _booksRepository;

        public CartRepository(IDBContext dBContext, IBooksRepository booksRepository)
        {
            _dbContext = dBContext;
            _booksRepository = booksRepository;
        }

        /// <summary>
        /// Inserts the specified book into cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>added cart item information</returns>
        public async Task<CartResponseDto> Insert(CartRequestDto cart, int userId)
        {
            CartResponseDto cartItem = null;
            using (SqlConnection connection = _dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand("sp_cart_insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@bookId", cart.BookId);
                command.Parameters.AddWithValue("@quantity", cart.Quantity);
                await connection.OpenAsync();
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItem = await MapReaderToCartDtoAsync(reader);
                    }
                }
                return cartItem;
            }
        }

        /// <summary>
        /// Deletes the specified book from cart.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>boolean result in 1/0 format</returns>
        public async Task<int> Delete(int bookId, int userId)
        {
            using (SqlConnection connection = _dbContext.GetConnection())
            {

                SqlCommand command = new SqlCommand("sp_cart_delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@bookId", bookId);
                await connection.OpenAsync();
                int isDeleted = await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return isDeleted;
            }
        }

        /// <summary>
        /// Updates the specified cart item.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Updated infomartion</returns>
        public async Task<CartResponseDto> Update(CartRequestDto cart, int userId)
        {
            CartResponseDto cartItem = null;
            using (SqlConnection connection = _dbContext.GetConnection())
            {

                SqlCommand command = new SqlCommand("sp_cart_update", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@bookId", cart.BookId);
                command.Parameters.AddWithValue("@quantity", cart.Quantity);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItem = await MapReaderToCartDtoAsync(reader);
                    }
                }
            }
            return cartItem;
        }

        /// <summary>
        /// Gets the all cart items.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<List<CartResponseDto>> Get(int userId)
        {
            List<CartResponseDto> cartItem = new List<CartResponseDto>();
            using (SqlConnection connection = _dbContext.GetConnection())
            {

                SqlCommand command = new SqlCommand("sp_cart_get_in_depth", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userid", userId);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItem.Add(await MapReaderToCartDtoAsync(reader));
                    }
                }
            }
            return cartItem;

        }

        /// <summary>
        /// Maps the reader to cart dto.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>CartResponseDto object </returns>
        private async Task<CartResponseDto> MapReaderToCartDtoAsync(SqlDataReader reader)
        {
            return new CartResponseDto
            {
                Book = _booksRepository.MapReaderTobook(reader),
                Total = (int)reader["total"],
                ItemQuantity = (int)reader["item_quantity"]
            };
        }
    }
}
