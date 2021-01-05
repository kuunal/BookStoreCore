using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.CartDto
{
    public class CartRequestDto
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Range(1, int.MaxValue)]
        public int BookId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

    }
}
