using Projector.Core.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests.TicketList
{
    public class CreateTicketListRequest : BaseRequest , ICreateResource
    {
        public string Name { get; set; }
        public Guid DefaultAssignedUser { get; set; }

    }
}
