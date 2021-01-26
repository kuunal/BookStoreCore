using ModelLayer.BookDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class BooksRepository : IBooksRepository
    {
        private readonly IDBContext _dBContext;

        public BooksRepository(IDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        /// <summary>
        /// Gets the paginated books from database.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="lastItemValue">The last item value.</param>
        /// <param name="sortby">The sortby.</param>
        /// <returns>List of book</returns>
        public async Task<List<BookResponseDto>> Get(string field, int limit, string lastItemValue, string sortby)
        {
            List<BookResponseDto> bookList = new List<BookResponseDto>();
            using (SqlConnection connection = _dBContext.GetConnection())
            {
                SqlCommand command = new SqlCommand("sp_books_get_paginated_result", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@limit", limit);
                command.Parameters.AddWithValue("@lastItemValue", lastItemValue);
                command.Parameters.AddWithValue("@field", field);
                command.Parameters.AddWithValue("@sortby", sortby);
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        bookList.Add(MapReaderTobook(reader));
                    }
                }
            }
            return bookList;
        }

        /// <summary>
        /// Maps the reader to BookResponseDto.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>BookResponseDto object</returns>
        public BookResponseDto MapReaderTobook(SqlDataReader reader)
        {
            return new BookResponseDto
            {
                Id = (int)reader["id"],
                Price = (int)reader["price"],
                Quantity = Convert.ToInt32(reader["quantity"]),
                Title = (string)reader["title"],
                Author = (string)reader["author"],
                Image = (string)reader["image"],
                Description = (string)reader["description"]
            };
        }

        /// <summary>
        /// Inserts the book into database.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <returns>Boolean result in format 1/0</returns>
        public async Task<int> Insert(BookRequestDto requestDto, string imageUrl)
        {
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_insert", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@description", requestDto.Description);
            command.Parameters.AddWithValue("@title", requestDto.Title);
            command.Parameters.AddWithValue("@author", requestDto.Author);
            command.Parameters.AddWithValue("@image", imageUrl);
            command.Parameters.AddWithValue("@price", requestDto.Price);
            command.Parameters.AddWithValue("@quantity", requestDto.Quantity);
            await connection.OpenAsync();
            int id = Convert.ToInt32(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return id;
        }

        /// <summary>
        /// Gets the specified book from database.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Book with given id</returns>
        public async Task<BookResponseDto> Get(int id)
        {
            BookResponseDto book = null;
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_detailed_view", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@id", id);
            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    book = MapReaderTobook(reader);
                }
            }
            await connection.CloseAsync();
            return book;
        }


        /// <summary>
        /// Deletes the specified book from databae.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>boolean reslt in format1/0</returns>
        public async Task<int> Delete(int id)
        {
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_delete", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@id", id);
            await connection.OpenAsync();
            int isDeleted = await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return isDeleted;
        }

        /// <summary>
        /// Updates the specified book.
        /// </summary>
        /// <param name="id">The book identifier.</param>
        /// <param name="requestDto">The request dto.</param>
        /// <returns>Updated book</returns>
        public async Task<BookResponseDto> Update(int id, BookRequestDto requestDto, string uploadedImageUrl)
        {
            BookResponseDto book = null;
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_update", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@description", requestDto.Description);
            command.Parameters.AddWithValue("@author", requestDto.Author);
            command.Parameters.AddWithValue("@title", requestDto.Title);
            command.Parameters.AddWithValue("@image", uploadedImageUrl);
            command.Parameters.AddWithValue("@price", requestDto.Price);
            command.Parameters.AddWithValue("@quantity", requestDto.Quantity);
            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    book = MapReaderTobook(reader);
                }
            }
            await connection.CloseAsync();
            return book;
        }

        public async Task<int> GetTotal()
        {
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_get_total_books", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            await connection.OpenAsync();
            int total = Convert.ToInt32(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return total;
        }

        public async Task<List<BookResponseDto>> GetSearchedBooks(string searchText)
        {
            List<BookResponseDto> book = new List<BookResponseDto>();
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_search_by_title", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@search_text", searchText);
            await connection.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    book.Add(MapReaderTobook(reader));
                }
            }
            await connection.CloseAsync();
            return book;
        }
    }
}
