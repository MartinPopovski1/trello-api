using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests.Users
{
    public class CreateUserRequest : BaseRequest , ICreateResource
    {
        
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        
        public List<Privilege> Privileges { get; set; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
