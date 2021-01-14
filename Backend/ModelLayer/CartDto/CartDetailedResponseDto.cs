using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.CartDto
{
    public class CartDetailedResponseDto
    {
        public List<CartResponseDto> cartItems { get; set; }
        public int Total { get; set; }
    }
}
