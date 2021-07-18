using Microsoft.Ajax.Utilities;
using Ninject.Activation;
using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Exceptions.ExtensionMethods;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Projector_Web_Api.Filters.ExceptionFilters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            
            if(actionExecutedContext.Exception.GetType() == typeof(BusinessException))
            {
                BusinessException exception = actionExecutedContext.Exception as BusinessException;
                
                actionExecutedContext.Response = actionExecutedContext.Request
                    .CreateResponse(exception.ValidationResult.StatusCode == null ? HttpStatusCode.InternalServerError : (HttpStatusCode)exception.ValidationResult.StatusCode, exception.ValidationResult);
            }
            else if (actionExecutedContext.Exception.GetType() == typeof(TechnicalException))
            {
                TechnicalException exception = actionExecutedContext.Exception as TechnicalException;
                ValidationResult validationResult = exception.Result;
                if(validationResult == null)
                {
                    validationResult = new ValidationResult(validationResult.Message , exception.StatusCode.Value);
                }
                actionExecutedContext.Response = actionExecutedContext.Request
                    .CreateResponse( validationResult.StatusCode == null ? HttpStatusCode.InternalServerError : (HttpStatusCode)validationResult.StatusCode, validationResult);
            }
            else
            {
                ValidationResult validationResult = actionExecutedContext.Exception.BuildValidationResult(HttpStatusCode.InternalServerError);
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, validationResult);
            }
        }
    }
}