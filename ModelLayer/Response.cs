﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
