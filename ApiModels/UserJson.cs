using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.ApiModels
{
    public class UserJson : BaseJsonModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PrivilegeJson> Privileges { get; set; }

    }
}