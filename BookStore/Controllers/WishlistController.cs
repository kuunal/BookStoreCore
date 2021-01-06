using BusinessLayer.Interface;
using Greeting.TokenAuthorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.WishlistDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TokenAuthorizationFilter]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _service;

        public WishlistController(IWishlistService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToWishList(WishlistDto wishlist)
        {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            WishlistDto addedWishlist = await _service.Insert(wishlist, userId);
            return Ok(new Response<WishlistDto>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = ResponseMessage.SUCCESSFUL,
                Data = addedWishlist
            });
        }

        [HttpGet]
         public async Task<IActionResult> GetWishlists()
         {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            List<WishlistDto> bookList = await _service.Get(userId);
            return Ok(new Response<List<WishlistDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = bookList
            });
         }

        [HttpDelete("bookId")]
        public async Task<IActionResult> RemoveFromWishList(int bookId)
        {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            int isDeleted = await _service.Delete(bookId, userId);
            if (isDeleted <= 0)
            {
                return NotFound(new Response<object>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = ResponseMessage.BOOK_REMOVE_FAILED,
                    Data = null
                });
            }
            return Ok(new Response<object>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.BOOK_REMOVE,
                Data = null
            });
        }
    }
}
