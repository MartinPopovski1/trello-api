using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Models
{
    public class UserPrivilege
    {
        public Guid UserId { get; set; }
        public Guid PrivilegeId { get; set; }
        public List<Privilege> Privileges { get; set; }
    }
}