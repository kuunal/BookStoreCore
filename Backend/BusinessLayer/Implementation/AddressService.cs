using BusinessLayer.Interface;
using ModelLayer.AddressDto;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;

        public AddressService(IAddressRepository repository)
        {
            _repository = repository;
        }

        public async Task<AddressResponseDto> GetAddress(int userId)
        {
            return await _repository.GetAddress(userId);
        }
    }
}
