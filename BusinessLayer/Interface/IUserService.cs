using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService
    {
        Task<UserResponseDto> AuthenticateUser(LoginDto loginDto); 
    }
}
