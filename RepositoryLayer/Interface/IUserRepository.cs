using ModelLayer;
using ModelLayer.UserDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        Task<UserResponseDto> AuthenticateUser(LoginDto loginDto);
        Task<UserResponseDto> Insert(UserRequestDto requestDto);

    }
}
