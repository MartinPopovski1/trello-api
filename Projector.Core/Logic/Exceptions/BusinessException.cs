using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Exceptions
{
    public class BusinessException : Exception
    {
        public ValidationResult ValidationResult;
        public BusinessException(ValidationResult result)
        {
            ValidationResult = result;
        }

    }
}
