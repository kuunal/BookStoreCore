using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using ModelLayer;
using ModelLayer.UserDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
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
            try
            {
            UserResponseDto response = null;
            int id = await _repository.Insert(requestDto);
            if (id > 0)
            {
               response = _mapper.Map<UserResponseDto>(requestDto);
            }
            response.Id = id;
            return response;
            }catch(SqlException e) when (e.Number == SqlErrorNumbers.DuplicateKey)
            {
                throw new BookstoreException(ExceptionMessages.ACCOUNT_ALREADY_EXISTS, (int)HttpStatusCode.BadRequest);
            }
        }
    }
}
