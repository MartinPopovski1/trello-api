using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            IsSuccessful = true;
            Errors = new List<ValidationError>();
        }
        public ValidationResult(string message , int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
            Errors = new List<ValidationError>();
            IsSuccessful = false;
        }
        public string Message { get; set; }
        public int? StatusCode { get; set; }
        public List<ValidationError> Errors { get; set; }
        public int? InternalCode { get; set; }
        public bool IsSuccessful { get; set; }

        public void AddErrorMessage(string message , dynamic info = null)
        {
            Errors.Add(new ValidationError { Message = message , Info = info});
        }
    }

    public class ValidationError
    {
        public string Message { get; set; }
        public dynamic Info { get; set; }

    }
}
