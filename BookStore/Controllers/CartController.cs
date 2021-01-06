using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.CartDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CartRequestDto cart)
        {
            int userId = (int) HttpContext.Items["userId"];
            CartResponseDto cartItem =  await _service.Insert(cart, userId);
            return Ok(new Response<CartResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = cartItem
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsFromCart()
        {
            int userId = (int)HttpContext.Items["userId"];
            List<CartResponseDto> cartItem = await _service.Get(userId);
            return Ok(new Response<List<CartResponseDto>>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = cartItem
            });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromCart(CartRequestDto cart)
        {
            int userId = (int)HttpContext.Items["userId"];
            int isDeleted = await _service.Delete(cart, userId);
            if (isDeleted == 0)
            {
                return BadRequest(new Response<CartResponseDto>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CART_ITEM_NOT_FOUND,
                    Data = null
                });
            }
            return Ok(new Response<CartResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = null
            });
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateInCart(CartRequestDto cart)
        {
            int userId = (int)HttpContext.Items["userId"];
            CartResponseDto cartItem = await _service.Update(cart, userId);
            if (cartItem == null)
            {
                return BadRequest(new Response<CartResponseDto>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = ResponseMessage.CART_ITEM_NOT_FOUND,
                    Data = null
                });
            }
            return Ok(new Response<CartResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = cartItem
            });
        }

    }
}
