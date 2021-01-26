using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.BookDto
{
    public class BookRequestDto
    {
        public string Description { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Price cannot be negative!")]
        public int Price { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity cannot be negative!")]
        public int Quantity { get; set; }
        
        public IFormFile? Image { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
    }
}
