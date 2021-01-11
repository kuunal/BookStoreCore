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

        /// <summary>
        /// Adds to wish list.
        /// </summary>
        /// <param name="wishlist">The wishlist dto.</param>
        /// <returns>readystate</returns>
        [HttpPost]
        public async Task<IActionResult> AddToWishList(WishlistDto wishlist)
        {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            WishlistResponseDto addedWishlist = await _service.Insert(wishlist, userId);
            return Ok(new Response<WishlistResponseDto>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = ResponseMessage.SUCCESSFUL,
                Data = addedWishlist
            });
        }

        /// <summary>
        /// Gets the wishlists items.
        /// </summary>
        /// <returns>readystate</returns>
        [HttpGet]
         public async Task<IActionResult> GetWishlists()
         {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            List<WishlistResponseDto> bookList = await _service.Get(userId);
            return Ok(new Response<List<WishlistResponseDto>>   
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = bookList
            });
         }

        /// <summary>
        /// Removes from wish list.
        /// </summary>
        /// <param name="bookId">The book identifier.</param>
        /// <returns>readystate or 404</returns>
        [HttpDelete("{bookId}")]
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
