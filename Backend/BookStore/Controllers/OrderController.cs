using BusinessLayer.Interface;
using Greeting.TokenAuthorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        [TokenAuthorizationFilter]
        public async Task<IActionResult> AddOrder(OrderRequestDto order)
        {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            OrderResponseDto orderResponse = await _service.Add(order, userId);
            return Ok(new Response<OrderResponseDto>
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = ResponseMessage.SUCCESSFUL,
                Data = orderResponse
            });
        }
    }
}
