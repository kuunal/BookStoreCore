using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.CartDto
{
    public class CartResponseDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

    }
}
