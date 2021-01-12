using AutoMapper;
using BusinessLayer.Exceptions;
using BusinessLayer.Interface;
using BusinessLayer.MQServices;
using EmailService;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using ModelLayer.UserDto;
using RepositoryLayer.Implementation;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        private readonly IConfiguration _configuration;
        private readonly IMqServices _mqServices;

        public UserService(IUserRepository repository
            , IMapper mapper
            , ITokenManager tokenManager
            , IConfiguration configuration
            , IMqServices mqServices)
        {
            _repository = repository;
            _mapper = mapper;
            _tokenManager = tokenManager;
            _configuration = configuration;
            _mqServices = mqServices;
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>logged in user or null value</returns>
        public async Task<(UserResponseDto, string)> AuthenticateUser(LoginDto loginDto)
        {
            var (user, password) = await _repository.AuthenticateUser(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, password))
            {
                return (null, null);
            }
            string token = _tokenManager.Encode(user);
            return (user, token);

        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <returns>Added user information</returns>
        /// <exception cref="BookstoreException">
        /// </exception>
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
            }catch(SqlException e) when (e.Number == SqlErrorNumbers.CONSTRAINT_VOILATION)
            {
                throw new BookstoreException(ExceptionMessages.INVALID_DATA, (int)HttpStatusCode.BadRequest);
            }
            catch (SqlException e) when (e.Number == SqlErrorNumbers.DUPLICATEKEY)
            {
                throw new BookstoreException(ExceptionMessages.ACCOUNT_ALREADY_EXISTS, (int)HttpStatusCode.BadRequest);
            }
        }

        public async Task ForgotPassword(string email, string currentUrl)
        {
            var (user, password) = await _repository.AuthenticateUser(email);

            if (user == null)
            {
                throw new BookstoreException(ExceptionMessages.INVALID_USER, 404);
            }
            byte[] secretKey = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["ResetPasswordSecretKey"]);
            int expiryTime = Convert.ToInt32(_configuration.GetSection("Jwt")["ExpiryTime"]);
            string jwt = _tokenManager.Encode(user, expiryTime, secretKey);
            string url = "https://" + currentUrl + "/html/reset.html?" + jwt;
            Message message = new Message(new string[] { user.Email },
                    "Password Reset Email",
                    $"<h6>Click on the link to reset password<h6><a href='{url}'>{jwt}</a>");
            _mqServices.AddToQueue(message);
        }

        public async Task<int> ResetPassword(string password, string token)
        {
            ClaimsPrincipal claims = _tokenManager.Decode(token, Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt")["ResetPasswordSecretKey"]));
            var claim = claims.Claims.ToList();
            string email = claim[1].Value;
            var(user, _) = await _repository.AuthenticateUser(email);
            return (await _repository.ResetPassword(user, BCrypt.Net.BCrypt.HashPassword(password)));
        }
    }
}
