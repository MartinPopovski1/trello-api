using Projector_Web_Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Projector_Web_Api.Filters
{
    public class GetUserAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var baseController = actionExecutedContext.ActionContext.ControllerContext.Controller as BaseApiController;
            //baseController.User = baseController.UserService.GetLogedInUser();
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}