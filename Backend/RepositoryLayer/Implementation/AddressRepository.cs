using ModelLayer.AddressDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDBContext _dBContext;

        public AddressRepository(IDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<AddressResponseDto> GetAddress(int userId)
        {
            AddressResponseDto address = null;
            SqlConnection connection = _dBContext.GetConnection();
            SqlCommand command = new SqlCommand("sp_address_get", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@userId", userId);
            await connection.OpenAsync();
            using(SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    address = MapReaderToAddressResponseDto(reader);
                }
            }
            await connection.CloseAsync();
            return address;  
        }

        private AddressResponseDto MapReaderToAddressResponseDto(SqlDataReader reader)
        {
            return new AddressResponseDto
            {
                id = (int)reader["id"],
                city = (string)reader["city"],
                house = (string)reader["house"],
                locality = (string)reader["locality"],
                pincode = (int)reader["pincode"],
                street = (string)reader["street"]
            };
        }
    }
}
