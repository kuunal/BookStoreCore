using ModelLayer.AddressDto;
using ModelLayer.OrderDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Implementation
{
    class OrderRepository : IOrderRepository
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

        public async Task<OrderResponseDto> Add(OrderRequestDto order, int userId)
        {
            OrderResponseDto orderResponse = null;
            using(SqlConnection connection = _dBContext.GetConnection())
            {
                await connection.OpenAsync();
                var transaction = await connection.BeginTransactionAsync();
                SqlCommand command = new SqlCommand("sp_orders_create")
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userId", userId);            
                command.Parameters.AddWithValue("@bookId", order.bookId);            
                command.Parameters.AddWithValue("@quantity", order.quantity);            
                command.Parameters.AddWithValue("@uuid", order.orderId);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    orderResponse.Book = _booksRepository.MapReaderTobook(reader);
                    orderResponse.User = _userRepository.MapUserFromReader(reader, "userId");
                    orderResponse.Address = MapReaderToAddressDto(reader);
                    orderResponse.OrderedDate = Convert.ToDateTime((string) reader["ordertime"]);
                    orderResponse.Quantity = (int) reader["orderedQuantity"];
                    orderResponse.OrderId = (string)reader["orderId"];
                }
                
            }
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
