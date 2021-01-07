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
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_insert", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@description", requestDto.Description);
            command.Parameters.AddWithValue("@title", requestDto.Title);
            command.Parameters.AddWithValue("@author", requestDto.Author);
            command.Parameters.AddWithValue("@image", requestDto.Image);
            command.Parameters.AddWithValue("@price", requestDto.Price);
            command.Parameters.AddWithValue("@quantity", requestDto.Quantity);
            await connection.OpenAsync();
            int id = Convert.ToInt32(await command.ExecuteScalarAsync());
            await connection.CloseAsync();
            return id;
        }

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

        public async Task<BookResponseDto> Update(int id, BookRequestDto requestDto)
        {
            BookResponseDto book = null;
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_books_update", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@id",id);
            command.Parameters.AddWithValue("@description", requestDto.Description);
            command.Parameters.AddWithValue("@author", requestDto.Author);
            command.Parameters.AddWithValue("@title", requestDto.Title);
            command.Parameters.AddWithValue("@image", requestDto.Image);
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

    }
}
