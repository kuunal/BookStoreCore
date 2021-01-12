using ModelLayer;
using ModelLayer.UserDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService
    {
        Task<(UserResponseDto, string)> AuthenticateUser(LoginDto loginDto);
        Task<UserResponseDto> AddUser(UserRequestDto requestDto);
        Task ForgotPassword(string email, string value);
        Task<int> ResetPassword(string password, string token);
    }
}
