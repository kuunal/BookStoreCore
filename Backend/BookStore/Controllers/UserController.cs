using BusinessLayer.Implementation;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.UserDto;
using System.Collections;
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

        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <param name="loginDto">Login Id and password</param>
        /// <returns>readystate or Unauthorized state</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var(user, token) = await _service.AuthenticateUser(loginDto);
            if (user != null)
            {
                return Ok(new Response<Hashtable>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = ResponseMessage.LOGIN_SUCCESS,
                    Data = new Hashtable { {"user", user }, { "token", token } }
                });
            }
            else { 
                return Unauthorized(new Response<UserResponseDto>
                 {
                     StatusCode = (int)HttpStatusCode.Unauthorized,
                     Message = ResponseMessage.LOGIN_FAILED,
                     Data = user
                 });
            }
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="requestDto">User data.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUser(UserRequestDto requestDto) 
        {
            UserResponseDto user = await _service.AddUser(requestDto);
            return Ok(new Response<UserResponseDto>
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = ResponseMessage.USER_ADDED,
                Data = user
            });
        }
    }
}
