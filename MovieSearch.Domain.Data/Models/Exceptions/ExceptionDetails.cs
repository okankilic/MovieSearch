using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Models.Exceptions
{
    public class ExceptionDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public ExceptionDetails()
        {

        }
    }
}
