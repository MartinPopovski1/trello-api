using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Projector_Web_Api.Controllers
{
    public class BaseApiController : ApiController
    {
        public User User { get; set; }
        public IUserService UserService { get; set; }

        public BaseApiController(IUserService userService) : base()
        {
            UserService = userService;
            User = userService.GetLogedInUser();
        }
    }
}