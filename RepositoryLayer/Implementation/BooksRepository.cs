﻿using ModelLayer.BookDto;
using RepositoryLayer.Interface;
using System;
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
    }
}