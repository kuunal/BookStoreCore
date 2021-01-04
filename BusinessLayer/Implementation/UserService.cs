using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> AuthenticateUser(LoginDto loginDto)
        {
            return await _repository.AuthenticateUser(loginDto);
        }

        public async Task<UserResponseDto> AddUser(UserRequestDto requestDto)
        {
            UserResponseDto response = null;
            int id = await _repository.Insert(requestDto);
            if (id > 0)
            {
               response = _mapper.Map<UserResponseDto>(requestDto);
            }
            response.Id = id;
            return response;
        }
    }
}
