using ModelLayer.AddressDto;
using ModelLayer.OrderDto;
using RepositoryLayer.Interface;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDBContext _dBContext;
        private readonly IUserRepository _userRepository;
        private readonly IBooksRepository _booksRepository;

        public OrderRepository(IDBContext dBContext, IUserRepository userRepository, IBooksRepository booksRepository)
        {
            _dBContext = dBContext;
            _userRepository = userRepository;
            _booksRepository = booksRepository;
        }

        public async Task<OrderResponseDto> Add(int userId, int bookId, int quantity, int addressId, string guid)
        {
            OrderResponseDto orderResponse = null;
            using(SqlConnection connection = _dBContext.GetConnection())
            {
                await connection.OpenAsync();
                var transaction = await connection.BeginTransactionAsync();
                try { 
                    SqlCommand command = new SqlCommand("sp_orders_create", connection, (SqlTransaction)transaction)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@userId", userId);            
                    command.Parameters.AddWithValue("@bookId", bookId);            
                    command.Parameters.AddWithValue("@quantity", quantity);            
                    command.Parameters.AddWithValue("@addressId", addressId);
                    command.Parameters.AddWithValue("@guid", guid);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync()) {
                            orderResponse = new OrderResponseDto();
                            orderResponse.Book = _booksRepository.MapReaderTobook(reader);
                            orderResponse.User = _userRepository.MapUserFromReader(reader, "userId");
                            orderResponse.Address = MapReaderToAddressDto(reader);
                            orderResponse.OrderedDate = Convert.ToDateTime(reader["ordertime"]);
                            orderResponse.Quantity = (int) reader["orderedQuantity"];
                            orderResponse.OrderId = (string)reader["orderId"];
                        }
                    }
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return orderResponse;
        }

        private AddressResponseDto MapReaderToAddressDto(SqlDataReader reader)
        {
            return new AddressResponseDto
            {
                city = (string)reader["city"],
                street = (string)reader["street"],
                house = (string)reader["house"],
                locality = (string)reader["locality"],
                pincode = (int)reader["pincode"],
            };
        }
    }
}
