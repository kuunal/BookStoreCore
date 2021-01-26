using Microsoft.AspNetCore.Mvc;
using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IBookService
    {
        Task<BookResponseDto> AddBook(BookRequestDto requestDto, string email);
        Task<List<BookResponseDto>> GetBooks(string field, int limit, string lastItemValue, string sortby);
        Task<BookResponseDto> Get(int id);
        Task<int> Delete(int id);
        Task<BookResponseDto> Update(int id, BookRequestDto requestDto, string email);
        Task<int> GetTotalNumberOfBooks();
        Task<List<BookResponseDto>> GetSearchedBooks(string searchText);
    }
}
