using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelLayer.BookDto
{
    public class BookRequestDto
    {
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        [Url]
        public string Image { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
    }
}
