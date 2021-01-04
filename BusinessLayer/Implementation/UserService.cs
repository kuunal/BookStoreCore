using BusinessLayer.Interface;
using ModelLayer;
using ModelLayer.UserDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserResponseDto> AuthenticateUser(LoginDto loginDto)
        {
            return await _repository.AuthenticateUser(loginDto);
        }

        public async Task<UserResponseDto> AddUser(UserRequestDto requestDto)
        {
            return await _repository.Insert(requestDto);
        }
    }
}
