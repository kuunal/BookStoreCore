using AutoMapper;
using ModelLayer;
using ModelLayer.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequestDto, UserResponseDto>();
        }
    }
}
