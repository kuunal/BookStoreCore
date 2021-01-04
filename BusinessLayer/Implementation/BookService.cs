﻿using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using ModelLayer.BookDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBooksRepository _repository;
        private readonly IMapper _mapper;

        public BookService(IBooksRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BookResponseDto> AddBook(BookRequestDto requestDto)
        {
            try
            {
                int id = await _repository.Insert(requestDto);
                BookResponseDto response = _mapper.Map<BookResponseDto>(requestDto);
                response.Id = id;
                return response;

            }catch(SqlException e) when(e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException("Invalid data");
            }
        }
    }
}