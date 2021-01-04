using ModelLayer;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _conn;
        public UserRepository(DBContext dBContext)
        {
            this._conn = dBContext.GetConnection();
        }

        public async Task<UserDto> AuthenticateUser(LoginDto loginDto)
        {
            UserDto user = null;
            SqlCommand command = new SqlCommand("sp_users_login", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@loginId", loginDto.Email);
            command.Parameters.AddWithValue("@password", loginDto.Password);
            await _conn.OpenAsync();
            using(SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    user = MapUserFromReader(reader);
                }
            }
            await _conn.CloseAsync();
            return user;
        }

        private UserDto MapUserFromReader(SqlDataReader reader)
        {
            return new UserDto
            {
                Id = (int)reader["id"],
                FirstName = (string)reader["firstname"],
                LastName = (string)reader["firstname"],
                PhoneNumber = (string)reader["phonenumber"],
                Email = (string)reader["email"],
                Role = (string) reader["role"]
            };
        }
    }
}
