﻿using ModelLayer;
using ModelLayer.UserDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        Task<(UserResponseDto, string)> AuthenticateUser(LoginDto loginDto);
        Task<int> Insert(UserRequestDto requestDto);

    }
}