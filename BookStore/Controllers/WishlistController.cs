using BusinessLayer.Interface;
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
            int userId = (int)HttpContext.Items["userId"];
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
            int userId = (int)HttpContext.Items["userId"];
            List<WishlistDto> bookList = await _service.Get(userId);
            return Ok(new Response<List<WishlistDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = bookList
            });
         }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromWishList(WishlistDto wishlist)
        {
            int userId = (int)HttpContext.Items["userId"];
            int isDeleted = await _service.Delete(wishlist, userId);
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
