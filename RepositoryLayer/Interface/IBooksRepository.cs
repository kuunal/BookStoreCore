using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IBooksRepository
    {
        Task<int> Insert(BookRequestDto requestDto);
        Task<List<BookResponseDto>> Get();
        Task<BookResponseDto> Get(int id);
        Task<int> Delete(int id);
    }
}
