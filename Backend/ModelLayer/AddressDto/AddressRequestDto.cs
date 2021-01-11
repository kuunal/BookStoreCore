using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.AddressDto
{
    public class AddressRequestDto
    {
        public string house { get; set; }
        public string street { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public int pincode { get; set; }
    }
}
