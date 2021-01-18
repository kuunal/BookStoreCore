using BusinessLayer.Interface;
using Greeting.TokenAuthorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.AddressDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [TokenAuthorizationFilter]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        // GET: api/<AddressController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = Convert.ToInt32(HttpContext.Items["userId"]);
            return Ok(new Response<AddressResponseDto>
            {
                StatusCode = 200,
                Message = ResponseMessage.SUCCESSFUL,
                Data = await _service.GetAddress(userId)
            });
        }
    }
}
