using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.OrderDto
{
    public class OrderRequestDto
    {
        [Range(1, int.MaxValue)]
        public int bookId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int quantity { get; set; }
        
        [Range(1, int.MaxValue)]
        public int addressId { get; set; }

    }
}
