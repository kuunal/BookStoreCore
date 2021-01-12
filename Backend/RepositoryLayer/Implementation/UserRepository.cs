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

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>user and password or null</returns>
        public async Task<(UserResponseDto, string)> AuthenticateUser(string email)
        {
            UserResponseDto user = null;
            string password = null;
            SqlCommand command = new SqlCommand("sp_users_getByEmail", _conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@email", email);
            await _conn.OpenAsync();
            using(SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    user = MapUserFromReader(reader);
                    password = (string) reader["password"];
                }
            }
            await _conn.CloseAsync();
            return (user, password);
        }

        /// <summary>
        /// Maps the user from reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>mapped object</returns>
        public UserResponseDto MapUserFromReader(SqlDataReader reader, string id = "id")
        {
            return new UserResponseDto
            {
                Id = (int)reader[id],
                FirstName = (string)reader["firstname"],
                LastName = (string)reader["lastname"],
                PhoneNumber = (string)reader["phonenumber"],
                Email = (string)reader["email"],
                Role = (string) reader["role"]
            };
        }

        /// <summary>
        /// Inserts the new user into database.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <returns>newly added user</returns>
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
