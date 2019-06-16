using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Models.Exceptions
{
    public class BusinessException: Exception
    {
        public BusinessException() : base()
        {

        }

        public BusinessException(string message) : base(message)
        {

        }

        public BusinessException(string formatString, params object[] args) : base(string.Format(formatString, args))
        {

        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
