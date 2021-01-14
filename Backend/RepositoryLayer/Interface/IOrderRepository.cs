using ModelLayer.AddressDto;
using ModelLayer.OrderDto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IOrderRepository
    {
        Task<OrderResponseDto> Add(int userId, int bookId, int quantity, int addressId, string guid);


    }
}
