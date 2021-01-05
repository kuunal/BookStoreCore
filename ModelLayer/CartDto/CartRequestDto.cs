using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.CartDto
{
    public class CartRequestDto
    {
        [Range(1, int.MaxValue)]
        public int userId { get; set; }

        [Range(1, int.MaxValue)]
        public int bookId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int quantity { get; set; }

    }
}
