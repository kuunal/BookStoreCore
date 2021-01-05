using ModelLayer.WishlistDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly SqlConnection _conn;

        public WishlistRepository(IDBContext dBContext)
        {
            _conn = dBContext.GetConnection();
        }

        public async Task<List<WishlistDto>> Get()
        {
            List<WishlistDto> wishlists = new List<WishlistDto>();
            SqlCommand command = new SqlCommand("sp_wishlist_get", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            await _conn.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    wishlists.Add(MapReaderToWishlist(reader));
                }
            }
            await _conn.CloseAsync();
            return wishlists;
        }

        private WishlistDto MapReaderToWishlist(SqlDataReader reader)
        {
            return new WishlistDto
            {
                BookId = (int) reader["bookId"],
                UserId = (int) reader["userId"]
            };
        }

        public async Task<int> Insert(WishlistDto wishlist)
        {
            SqlCommand command = new SqlCommand("sp_wishlist_insert", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", wishlist.UserId);
            command.Parameters.AddWithValue("@bookId", wishlist.BookId);
            await _conn.OpenAsync();
            int isInserted = await command.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            return isInserted;
        }

        public async Task<int> Delete(WishlistDto wishlist)
        {
            SqlCommand command = new SqlCommand("sp_wishlist_insert", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", wishlist.UserId);
            command.Parameters.AddWithValue("@bookId", wishlist.BookId);
            await _conn.OpenAsync();
            int isDeleted = await command.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            return isDeleted;
        }
    }
}
