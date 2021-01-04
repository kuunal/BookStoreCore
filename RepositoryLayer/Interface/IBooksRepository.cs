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
    }
}
