using ModelLayer;
using ModelLayer.UserDto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        Task<(UserResponseDto, string)> AuthenticateUser(string email);
        Task<int> Insert(UserRequestDto requestDto);
        UserResponseDto MapUserFromReader(SqlDataReader reader, string id = "id");

    }
}
