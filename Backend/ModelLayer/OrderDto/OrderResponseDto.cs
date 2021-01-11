using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.OrderDto
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public BookResponseDto Book { get; set; }
        public UserResponseDto User  { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderedDate { get; set; }
        public string OrderId { get; set; }
        public AddressDto Address { get; set; }
    }
}
