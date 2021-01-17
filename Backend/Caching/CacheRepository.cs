using Caching;
using ModelLayer.CartDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IResponseCacheService _service;

        public CacheRepository(IResponseCacheService service)
        {
            _service = service;
        }
        public async Task AddAsync(string key, CartResponseDto cart)
        {
            CartDetailedResponseDto cartDetailed = JsonConvert
                                        .DeserializeObject<CartDetailedResponseDto>(await _service
                                        .GetCachedResponseAsync(key)
                                        );
            if (cartDetailed == null)
                return;
            cartDetailed.cartItems.Add(cart);
            await _service.CacheResponseAsync(key, cartDetailed, TimeSpan.FromSeconds(600));
        }

        public async Task DeleteAsync(string key, int bookId)
        {
            CartDetailedResponseDto cartDetailed = JsonConvert
                                                    .DeserializeObject<CartDetailedResponseDto>(await _service
                                                    .GetCachedResponseAsync(key)
                                                    );
            if (cartDetailed == null)
                return;
            cartDetailed.cartItems.Remove(cartDetailed.cartItems.FirstOrDefault(item=>item.Book.Id == bookId));
            await _service.CacheResponseAsync(key, cartDetailed, TimeSpan.FromSeconds(600));        
        }

        public async Task Get(string key, CartDetailedResponseDto response)
        {
            if (response == null)
                return;
            await _service.CacheResponseAsync(key, response, TimeSpan.FromSeconds(599));
        }

        public async Task UpdateAsync(string key, CartResponseDto requestDto, int bookId)
        {
            CartDetailedResponseDto cartDetailed = JsonConvert
                                        .DeserializeObject<CartDetailedResponseDto>(await _service
                                        .GetCachedResponseAsync(key)
                                        );
            if (cartDetailed == null)
                return;
            foreach(var item in cartDetailed.cartItems.Where(cart=>cart.Book.Id == bookId))
            {
                item.ItemQuantity = requestDto.ItemQuantity;
            }            
            await _service.CacheResponseAsync(key
                , cartDetailed
                , TimeSpan.FromSeconds(600));
        }
    }
}
