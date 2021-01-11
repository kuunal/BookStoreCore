using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.OrderDto
{
    public class OrderResponseDto
    {
        public int id { get; set; }
        public int bookId { get; set; }
        public int userId { get; set; }
        public int quantity { get; set; }
        public DateTime orderedDate { get; set; }
        public string orderId { get; set; }
    }
}
