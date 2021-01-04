using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService
    {
        Task<UserDto> AuthenticateUser(LoginDto loginDto); 
    }
}
