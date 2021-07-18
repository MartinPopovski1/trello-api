using AutoMapper;
using Castle.Components.DictionaryAdapter;
using Castle.Components.DictionaryAdapter.Xml;
using Ninject;
using Ninject.Web.Common;
using Projector.Core.Logic.Handlers.BoardHandler;
using Projector.Core.Logic.Handlers.ProjectHandler;
using Projector.Core.Logic.Handlers.TicketHandler;
using Projector.Core.Logic.Handlers.TicketListHandler;
using Projector.Core.Logic.Handlers.UserHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Board;
using Projector.Core.Logic.Requests.Project;
using Projector.Core.Logic.Requests.Ticket;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Logic.Requests.Users;
using Projector.Core.Logic.Services.UserService;
using Projector.Core.Logic.Validators.BoardValidators;
using Projector.Core.Logic.Validators.ProjectValidators;
using Projector.Core.Logic.Validators.TicketListValidators;
using Projector.Core.Logic.Validators.TicketValidator;
using Projector.Core.Logic.Validators.UserValidators;
using Projector.Core.Models;
using Projector.Repositories.SqlClientRepositories;
using Projector_Web_Api.ApiModels;
using Projector_Web_Api.Mappers;
using Projector_Web_Api.Mappers.Profiles;
using System;
using System.CodeDom;
using System.Diagnostics;

namespace Projector_Web_Api.Configuration.Bootstrap
{
    public class NinjectBootstrap
    {
        public static IKernel Bootstrap()
        {
            var kernel = new StandardKernel();

           
            

            // Project Bindings /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            kernel.Bind<ProjectRepository>().ToSelf().WithConstructorArgument("connectionString", Startup.ConnectionString);

            kernel.Bind<IProjectRepository>().ToMethod(c => kernel.Get<ProjectRepository>()).InTransientScope();

            kernel.Bind<IProjectValidator>().To<ProjectValidator>().InTransientScope();

            kernel.Bind<ProjectHandler>().ToSelf();

            kernel.Bind<IProjectHandler>().To<ProjectHandler>().InTransientScope();

            // Board Bindings /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            kernel.Bind<BoardRepository>().ToSelf().WithConstructorArgument("connectionString", Startup.ConnectionString);

            kernel.Bind<IBoardRepository>().ToMethod(c => kernel.Get<BoardRepository>()).InTransientScope();

            kernel.Bind<IBoardValidator>().To<BoardValidator>().InTransientScope();

            kernel.Bind<BoardHandler>().ToSelf();

            kernel.Bind<IBoardHandler>().To<BoardHandler>().InTransientScope();

            // Ticket List Bindings /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            kernel.Bind<TicketListRepository>().ToSelf().WithConstructorArgument("connectionString", Startup.ConnectionString);

            
            kernel.Bind<ITicketListRepository>().ToMethod(c => kernel.Get<TicketListRepository>()).InTransientScope();

            kernel.Bind<ITicketListValidator>().To<TicketListValidator>().InTransientScope();

            kernel.Bind<TicketListHandler>().ToSelf();

            kernel.Bind<ITicketListHandler>().To<TicketListHandler>().InTransientScope();

            // Ticket Bindings /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            kernel.Bind<TicketRepository>().ToSelf().WithConstructorArgument("connectionString", Startup.ConnectionString);

            kernel.Bind<ITicketRepository>().ToMethod(c => kernel.Get<TicketRepository>()).InTransientScope();

            kernel.Bind<ITicketValidator>().To<TicketValidator>().InTransientScope();

            kernel.Bind<TicketHandler>().ToSelf();

            kernel.Bind<ITicketHandler>().To<TicketHandler>().InTransientScope();

            // User Bindings /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            kernel.Bind<UserRepository>().ToSelf().WithConstructorArgument("connectionString", Startup.ConnectionString);

            kernel.Bind<IUserRepository>().ToMethod(c => kernel.Get<UserRepository>()).InTransientScope();

            kernel.Bind<UserValidator>().ToSelf();

            kernel.Bind<IUserValidator>().To<UserValidator>().InTransientScope();

            kernel.Bind<UserHandler>().ToSelf();

            kernel.Bind<IUserHandler>().To<UserHandler>().InRequestScope();

            kernel.Bind<IUserService>().To<UserService>().InRequestScope();



            // AutoMapper Binding /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToJsonMaps>();
                cfg.AddProfile<ProjectRequestProfile>();
                cfg.AddProfile<BoardRequestProfile>();
                cfg.AddProfile<TicketListRequestProfile>();
                cfg.AddProfile<TicketRequestProfile>();
                cfg.AddProfile<UserRequestProfile>();

            });


            kernel.Bind<IMapper>().ToConstructor(c => new Mapper(mapperConfiguration)).InSingletonScope();

            kernel.Bind(typeof(IMapJsonToDomain<ProjectJson, Guid>)).To(typeof(GenericJsonToDomainMapper<ProjectJson, Guid, CreateProjectRequest, EditProjectRequest, BaseIdentifyRequest<Guid>>)).InTransientScope();
            kernel.Bind(typeof(IMapJsonToDomain<BoardJson, Guid>)).To(typeof(GenericJsonToDomainMapper<BoardJson, Guid, CreateBoardRequest, EditBoardRequest, BaseIdentifyRequest<Guid>>)).InTransientScope();
            kernel.Bind(typeof(IMapJsonToDomain<TicketListJson, Guid>)).To(typeof(GenericJsonToDomainMapper<TicketListJson, Guid, CreateTicketListRequest, EditTicketListRequest, BaseIdentifyRequest<Guid>>)).InTransientScope();
            kernel.Bind(typeof(IMapJsonToDomain<TicketJson, Guid>)).To(typeof(GenericJsonToDomainMapper<TicketJson, Guid, CreateTicketRequest, EditTicketRequest, BaseIdentifyRequest<Guid>>)).InTransientScope();
            kernel.Bind(typeof(IMapJsonToDomain<UserJson, Guid>)).To(typeof(GenericJsonToDomainMapper<UserJson, Guid, CreateUserRequest, EditUserRequest, BaseIdentifyRequest<Guid>>)).InTransientScope();

            kernel.Bind(typeof(IMapDomainToJson<Project, ProjectJson>)).To(typeof(GenericDomainToJsonMapper<Project, ProjectJson>)).InTransientScope();
            kernel.Bind(typeof(IMapDomainToJson<Board, BoardJson>)).To(typeof(GenericDomainToJsonMapper<Board, BoardJson>)).InTransientScope();
            kernel.Bind(typeof(IMapDomainToJson<TicketList, TicketListJson>)).To(typeof(GenericDomainToJsonMapper<TicketList, TicketListJson>)).InTransientScope();
            kernel.Bind(typeof(IMapDomainToJson<Ticket, TicketJson>)).To(typeof(GenericDomainToJsonMapper<Ticket, TicketJson>)).InTransientScope();
            kernel.Bind(typeof(IMapDomainToJson<User, UserJson>)).To(typeof(GenericDomainToJsonMapper<User, UserJson>)).InTransientScope();
            kernel.Bind(typeof(IMapDomainToJson<Privilege, PrivilegeJson>)).To(typeof(GenericDomainToJsonMapper<Privilege, PrivilegeJson>)).InTransientScope();

            return kernel;
        }
    }
}