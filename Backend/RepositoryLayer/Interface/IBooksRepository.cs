using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IBooksRepository
    {
        Task<int> Insert(BookRequestDto requestDto);
        Task<List<BookResponseDto>> Get(string field, int limit, string lastItemValue, string sortby);
        Task<BookResponseDto> Get(int id);
        Task<int> Delete(int id);
        Task<BookResponseDto> Update(int id, BookRequestDto requestDto);
        Task<int> GetTotal();
        BookResponseDto MapReaderTobook(SqlDataReader reader);

    }
}
