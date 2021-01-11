using ModelLayer.BookDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.CartDto
{
    public class CartResponseDto
    {
        public BookResponseDto Book { get; set; }
        public int ItemQuantity { get; set; }
        public int Total { get; set; }

    }
}
