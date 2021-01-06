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
                        cartItem = MapReaderToCartDto(reader);
                    }
                }
                return cartItem;
            }
        }

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
                        cartItem = MapReaderToCartDto(reader);
                    }
                }
            }
            return cartItem;
        }

        public async Task<List<CartResponseDto>> Get(int userId)
        {
            List<CartResponseDto> cartItem = new List<CartResponseDto>();
            using (SqlConnection connection = _dbContext.GetConnection())
            {

                SqlCommand command = new SqlCommand("sp_cart_get", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userid", userId);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        cartItem.Add(MapReaderToCartDto(reader));
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
                Price = (int)reader["price"],
                Quantity = (int)reader["quantity"]
            };
        }
    }
}
