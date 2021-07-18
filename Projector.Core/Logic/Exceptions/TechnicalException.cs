using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Exceptions
{
    public class TechnicalException : Exception
    {
        public ValidationResult Result;
        public int? StatusCode { get; set; }
        public TechnicalException(ValidationResult result)
        {
            Result = result;

        }
        public TechnicalException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
