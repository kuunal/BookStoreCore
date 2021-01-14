using ModelLayer.CartDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Caching
{
    public interface ICacheRepository
    {
        Task Get(string key, CartDetailedResponseDto response);
        Task UpdateAsync(string key, CartResponseDto requestDto, int bookId);
        Task AddAsync(string key, CartResponseDto cart);
        Task DeleteAsync(string key, int bookId);

    }
}
