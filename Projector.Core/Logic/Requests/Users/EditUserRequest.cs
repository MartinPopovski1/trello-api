using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using Projector.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests.Users
{
    public class EditUserRequest : BaseRequest , IEditResource
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Privilege> Privileges { get; set; }
        public Guid Id { get; set; }
    }
}
