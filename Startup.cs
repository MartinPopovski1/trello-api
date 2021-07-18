using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;
using Projector.Core.Logic.Handlers.UserHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests.Users;
using Projector.Core.Logic.Services.UserService;
using Projector.Core.Models;
using Projector.Repositories.SqlClientRepositories;
using Projector_Web_Api;
using Projector_Web_Api.Configuration;
using Projector_Web_Api.Configuration.Bootstrap;
using Projector_Web_Api.Filters;
using Projector_Web_Api.Filters.ExceptionFilters;
using Projector_Web_Api.Oauth.AutohrizationServer;
using Projector_Web_Api.Utilities;
using CorsOptions = Microsoft.Owin.Cors.CorsOptions;

[assembly: Microsoft.Owin.OwinStartup(typeof(Startup))]
namespace Projector_Web_Api
{
    public class Startup
    {
        public static IKernel Kernel;
        public static string ConnectionString;
        public Startup()
        {
            // Application Settings 
            // Connection Strings 
            // Mapper 
            // IOC Container 

            ConnectionString = ConfigurationManager.ConnectionStrings["ProjectorConnString"].ConnectionString;
            IUserRepository userRepository = new UserRepository(ConnectionString);
            var users = userRepository.GetAll().Result;
            if (users.Count == 0)
            {
                IUserService userService = new UserService(userRepository);
                var user = userService.Create(new CreateUserRequest
                {
                    
                    FirstName = "Aleksandar",
                    LastName = "Lazarov",
                    UserName = "Lazaleks",
                }, "magdonos");
                //var user = userService.Create(new Projector.Core.Models.User
                //{
                //    Id = Guid.NewGuid(),
                //    FirstName = "Aleksandar",
                //    LastName = "Lazarov",
                //    UserName = "Lazaleks",
                //    IsActive = true
                //}, "magdonos");
               // userRepository.CreateUser(user).Wait();
            }

            Kernel = NinjectBootstrap.Bootstrap();
        }

        // Konfiguracija na Web Server
        // Routings
        // Auth & Autho
        // Filtri
        // Logging
        public void Configuration(IAppBuilder app)
        {
            
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            //httpConfiguration.Filters.Add(new AuthorizeAttribute());
            System.Web.Http.Dependencies.IDependencyResolver ninjectResolver = new NinjectResolver(Kernel);

            httpConfiguration.DependencyResolver = ninjectResolver;
            GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;

            httpConfiguration.Filters.Add(new GlobalExceptionFilter());

            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new LowercaseContractResolver();
            httpConfiguration.SuppressDefaultHostAuthentication();

            var oauthAuthoServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/api/login"),
                Provider = new ApplicationAuthProvider(new UserService(new UserRepository(ConnectionString))),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                AllowInsecureHttp = true,
            };


            var oauthBearerAuthOptions = new OAuthBearerAuthenticationOptions()
            {
                Provider = new OAuthBearerAuthenticationProvider()
                {

                }
            };

            //httpConfiguration.Filters.Add(new CustomAuthorizationAttribute());
            
            app.UseCors(CorsOptions.AllowAll);

            app.UseOAuthAuthorizationServer(oauthAuthoServerOptions);
            app.UseOAuthBearerAuthentication(oauthBearerAuthOptions);
            

            
            app.UseWebApi(httpConfiguration);
            
        }
    }
}