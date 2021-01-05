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

        public async Task<int> Insert(WishlistDto wishlist)
        {
            SqlCommand command = new SqlCommand("sp_wishlist_insert", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", wishlist.userId);
            command.Parameters.AddWithValue("@bookId", wishlist.bookId);
            await _conn.OpenAsync();
            int isInserted = await command.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            return isInserted;
        }
    }
}
