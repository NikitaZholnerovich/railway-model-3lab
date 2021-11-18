using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
