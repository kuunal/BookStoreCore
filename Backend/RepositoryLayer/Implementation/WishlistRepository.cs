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
        private readonly IDBContext _dbContext;
        private readonly IBooksRepository _booksRepository;

        public WishlistRepository(IDBContext dBContext, IBooksRepository booksRepository)
        {
            _dbContext = dBContext;
            _booksRepository = booksRepository;
        }

        /// <summary>
        /// Gets the wishlist for user id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Wishilist for loggedin user</returns>
        public async Task<List<WishlistResponseDto>> Get(int userId)
        {
            List<WishlistResponseDto> wishlists = new List<WishlistResponseDto>();
            SqlConnection _conn = _dbContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_wishlist_get", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", userId);
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

        /// <summary>
        /// Maps the reader to wishlist.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private WishlistResponseDto MapReaderToWishlist(SqlDataReader reader)
        {
            return new WishlistResponseDto
            {
                Book = _booksRepository.MapReaderTobook(reader)           
            };
        }

        /// <summary>
        /// Inserts the book into wishlist.
        /// </summary>
        /// <param name="wishlist">The wishlist.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<WishlistResponseDto> Insert(WishlistDto wishlist, int userId)
        {
            WishlistResponseDto addedItem = null;
            SqlConnection _conn = _dbContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_wishlist_insert", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@bookId", wishlist.BookId);
            await _conn.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    addedItem = MapReaderToWishlist(reader);
                }
            }
            await _conn.CloseAsync();
            return addedItem;
        }

        /// <summary>
        /// Deletes the specified book id from wishlist.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>boolean result in 1/0 format</returns>
        public async Task<int> Delete(int bookId, int userId)
        {
            SqlConnection _conn = _dbContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_wishlist_delete", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@bookId", bookId);
            await _conn.OpenAsync();
            int isDeleted = await command.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            return isDeleted;
        }
    }
}
