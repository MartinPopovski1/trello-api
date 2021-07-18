using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Exceptions.ExtensionMethods
{
    public static class ExtentionMethods 
    {
        public static ValidationResult BuildValidationResult(this Exception exception , HttpStatusCode statusCode)
        {
            ValidationResult validationResult = new ValidationResult("Unexpected Error !", (int)statusCode);
            Exception _exception = exception;
            
            while (true)
            {
                if (_exception == null)
                    break;

                validationResult.Errors.Add(new ValidationError
                {
                    Message = _exception.Message
                });

                _exception = _exception.InnerException;
            }
            return validationResult;
        }
    }


        
}
