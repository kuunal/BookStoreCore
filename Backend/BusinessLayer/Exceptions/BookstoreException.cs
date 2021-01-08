using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Exceptions
{
    public class BookstoreException : Exception
    {
        public string Message;
        public int StatusCode;
        public BookstoreException(string message, int statusCode = 400) : base(message)
        {
            this.Message = message;
            this.StatusCode = statusCode;
        }
        public override string ToString()
        {
            return StatusCode.ToString();
        }
    }
}
