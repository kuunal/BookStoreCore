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

        public CartRepository(IDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<CartResponseDto> Insert(CartRequestDto cart)
        {
            CartResponseDto cartItem = null;
            using (SqlConnection connection = _dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand("sp_cart_insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", cart.UserId);
                command.Parameters.AddWithValue("@bookId", cart.BookId);
                command.Parameters.AddWithValue("@quantity", cart.Quantity);
                await connection.OpenAsync();
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItem = MapReaderToCartDto(reader);
                    }
                }
                return cartItem;
            }
        }

        public async Task<int> Delete(CartRequestDto cart)
        {
            using (SqlConnection connection = _dbContext.GetConnection())
            {

                SqlCommand command = new SqlCommand("sp_wishlist_delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", cart.UserId);
                command.Parameters.AddWithValue("@bookId", cart.BookId);
                await connection.OpenAsync();
                int isDeleted = await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
                return isDeleted;
            }
        }

        public async Task<CartResponseDto> Update(CartRequestDto cart)
        {
            CartResponseDto cartItem = null;
            using (SqlConnection connection = _dbContext.GetConnection())
            {

                SqlCommand command = new SqlCommand("sp_wishlist_delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", cart.UserId);
                command.Parameters.AddWithValue("@bookId", cart.BookId);
                command.Parameters.AddWithValue("@quantity", cart.Quantity);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItem = MapReaderToCartDto(reader);
                    }
                }
            }
            return cartItem;
        }

        private CartResponseDto MapReaderToCartDto(SqlDataReader reader)
        {
            return new CartResponseDto
            {
                BookId = (int)reader["bookId"],
                UserId = (int)reader["userId"],
                Price = (int)reader["price"],
                Quantity = (int)reader["quantity"]
            };
        }
    }
}
