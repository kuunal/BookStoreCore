using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IBookService
    {
        Task<BookResponseDto> AddBook(BookRequestDto requestDto);
        Task<List<BookResponseDto>> GetBooks();
        Task<BookResponseDto> Get(int id);
        Task<int> Delete(int id);
    }
}
