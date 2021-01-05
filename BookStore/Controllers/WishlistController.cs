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
            WishlistDto addedWishlist = await _service.Insert(wishlist);
            return Ok(new Response<WishlistDto>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = ResponseMessage.SUCCESSFUL,
                Data = addedWishlist
            });
        }
    }
}
