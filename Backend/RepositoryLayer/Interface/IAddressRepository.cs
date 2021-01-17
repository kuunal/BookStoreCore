using ModelLayer.AddressDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAddressRepository
    {
        Task<AddressResponseDto> GetAddress(int userId);
    }
}
