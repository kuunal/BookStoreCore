using ModelLayer.AddressDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressService
    {
        Task<AddressResponseDto> GetAddress(int userId);
    }
}
