using ModelLayer;
using ModelLayer.UserDto;
using RepositoryLayer.Interface;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _conn;

        public UserRepository(IDBContext dBContext)
        {
            this._conn = dBContext.GetConnection();
        }

        public async Task<UserResponseDto> AuthenticateUser(LoginDto loginDto)
        {
            UserResponseDto user = null;
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

        private UserResponseDto MapUserFromReader(SqlDataReader reader)
        {
            return new UserResponseDto
            {
                Id = (int)reader["id"],
                FirstName = (string)reader["firstname"],
                LastName = (string)reader["firstname"],
                PhoneNumber = (string)reader["phonenumber"],
                Email = (string)reader["email"],
                Role = (string) reader["role"]
            };
        }
        
        public async Task<int> Insert(UserRequestDto requestDto)
        {
            SqlCommand command = new SqlCommand("sp_users_insert", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@firstname", requestDto.FirstName);
            command.Parameters.AddWithValue("@lastname", requestDto.LastName);
            command.Parameters.AddWithValue("@email", requestDto.Email);
            command.Parameters.AddWithValue("@phonenumber", requestDto.PhoneNumber);
            command.Parameters.AddWithValue("@password", requestDto.Password);
            command.Parameters.AddWithValue("@role", requestDto.Role);
            await _conn.OpenAsync();
            int id = Convert.ToInt32(await command.ExecuteScalarAsync());
            return id;
        }
    }
}
