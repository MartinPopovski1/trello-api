using Microsoft.Owin.Security.OAuth;
using Projector.Core.Logic.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projector_Web_Api.Oauth.AutohrizationServer
{
    public class ApplicationAuthProvider : OAuthAuthorizationServerProvider
    {
        private IUserService _userService;
        public ApplicationAuthProvider(IUserService userService)
        {
            _userService = userService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var user = await _userService.Authenticate(context.UserName, context.Password);

                if (!user.IsActive)
                    throw new Exception("The user is not active");

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
                //identity.AddClaim(new Claim(ClaimTypes.pas, context.Password));

                //identity.AddClaim(new Claim("Role", context))

                context.Validated(identity);
                var ticket = context.Ticket;

            }
            catch (Exception e)
            {
                context.SetError(e.Message);
            }
        }
    }
}