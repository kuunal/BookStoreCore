using BusinessLayer.Implementation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            UserDto user = await _service.AuthenticateUser(loginDto);
            if (user != null)
            {
                return Ok(new Response<UserDto>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = ResponseMessage.LOGIN_SUCCESS,
                    Data = user
                });
            }
            else { 
                return Unauthorized(new Response<UserDto>
                 {
                     StatusCode = (int)HttpStatusCode.Unauthorized,
                     Message = ResponseMessage.LOGIN_SUCCESS,
                     Data = user
                 });
            }
        }
    }
}
