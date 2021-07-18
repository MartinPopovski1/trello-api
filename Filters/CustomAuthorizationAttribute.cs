using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Projector_Web_Api.Filters
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            var userId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            IUserService userService = actionContext.Request.GetDependencyScope().GetService(typeof(IUserService)) as IUserService;
            //var user = await userService.GetUser(Guid.Parse(userId.ToString()));
            var user = await userService.GetUser(new BaseIdentifyRequest<Guid> { Id = Guid.Parse(userId.ToString()) }); ;
            userService.SetUser(user);

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
        }
    }
}