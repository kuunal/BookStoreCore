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
        private readonly SqlConnection _conn;

        public BooksRepository(IDBContext dBContext)
        {
            _conn = dBContext.GetConnection();
        }

        public async Task<List<BookResponseDto>> Get()
        {
            List<BookResponseDto> bookList = new List<BookResponseDto>();
            SqlCommand command = new SqlCommand("sp_books_get", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            await _conn.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    bookList.Add(MapReaderTobook(reader));
                }
            }
            await _conn.CloseAsync();
            return bookList;
        }

        private BookResponseDto MapReaderTobook(SqlDataReader reader)
        {
            return new BookResponseDto
            {
                Id = (int)reader["id"],
                Price = (int)reader["price"],
                Quantity = Convert.ToInt32(reader["quantity"]),
                Title = (string)reader["title"],
                Author = (string)reader["author"],
                Image = (string)reader["image"],
                Description = (string) reader["description"]
            };
        }

        public async Task<int> Insert(BookRequestDto requestDto)
        {
            SqlCommand command = new SqlCommand("sp_books_insert", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@description", requestDto.Description);
            command.Parameters.AddWithValue("@title", requestDto.Title);
            command.Parameters.AddWithValue("@author", requestDto.Author);
            command.Parameters.AddWithValue("@image", requestDto.Image);
            command.Parameters.AddWithValue("@price", requestDto.Price);
            command.Parameters.AddWithValue("@quantity", requestDto.Quantity);
            await _conn.OpenAsync();
            int id = Convert.ToInt32(await command.ExecuteScalarAsync());
            await _conn.CloseAsync();
            return id;
        }

        public async Task<BookResponseDto> Get(int id)
        {
            BookResponseDto book = null;
            SqlCommand command = new SqlCommand("sp_books_detailed_view", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@id", id);
            await _conn.OpenAsync();
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    book = MapReaderTobook(reader);
                }
            }
            await _conn.CloseAsync();
            return book;
        }

        public async Task<int> Delete(int id)
        {
            SqlCommand command = new SqlCommand("sp_books_delete", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@id", id);
            await _conn.OpenAsync();
            int isDeleted = await command.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            return isDeleted;
        }
    }
}
