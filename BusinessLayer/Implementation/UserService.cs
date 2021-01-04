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
using TokenAuthorization;

namespace BusinessLayer.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;

        public UserService(IUserRepository repository, IMapper mapper, ITokenManager tokenManager)
        {
            _repository = repository;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }

        public async Task<(UserResponseDto, string)> AuthenticateUser(LoginDto loginDto)
        {
            var (user, password) = await _repository.AuthenticateUser(loginDto);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, password))
            {
                return (null, null);
            }
            string token = _tokenManager.Encode(user);
            return (user, token);

        }

        public async Task<UserResponseDto> AddUser(UserRequestDto requestDto)
        {
            try
            {
            UserResponseDto response = null;
            requestDto.Password = BCrypt.Net.BCrypt.HashPassword(requestDto.Password);
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
