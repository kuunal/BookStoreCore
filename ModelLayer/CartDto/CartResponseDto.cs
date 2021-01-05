using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.CartDto
{
    public class CartResponseDto
    {
        public int userId { get; set; }
        public int bookId { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }

    }
}
