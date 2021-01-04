using BusinessLayer.Implementation;
using BusinessLayer.Interface;
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
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            UserResponseDto user = await _service.AuthenticateUser(loginDto);
            if (user != null)
            {
                return Ok(new Response<UserResponseDto>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = ResponseMessage.LOGIN_SUCCESS,
                    Data = user
                });
            }
            else { 
                return Unauthorized(new Response<UserResponseDto>
                 {
                     StatusCode = (int)HttpStatusCode.Unauthorized,
                     Message = ResponseMessage.LOGIN_SUCCESS,
                     Data = user
                 });
            }
        }
    }
}
