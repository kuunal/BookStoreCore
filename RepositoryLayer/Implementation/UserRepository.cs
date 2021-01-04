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
        private SqlConnection _conn;
        public UserRepository(DBContext dBContext)
        {
            this._conn = dBContext.GetConnection();
        }

        public async Task<UserDto> AuthenticateUser(LoginDto loginDto)
        {
            UserDto user = null;
            SqlCommand command = new SqlCommand("sp_users_login", _conn);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@loginId", loginDto.Email);
            command.Parameters.AddWithValue("@password", loginDto.Password);
            await _conn.OpenAsync();
            using(SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    user = mapUserFromReader(reader);
                }
            }
            _conn.Close();
            return user;
        }

        private UserDto mapUserFromReader(SqlDataReader reader)
        {
            return new UserDto
            {
                Id = (int)reader["id"],
                FirstName = (string)reader["firstname"],
                LastName = (string)reader["firstname"],
                PhoneNumber = (string)reader["phonenumber"],
                Email = (string)reader["email"],
                role = (string) reader["role"]
            };
        }
    }
}
