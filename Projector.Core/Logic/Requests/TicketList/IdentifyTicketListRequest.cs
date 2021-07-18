using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests.TicketList
{
    public class IdentifyTicketListRequest : BaseIdentifyRequest<Guid> , IIdentifyResource<Guid>
    {
        public Guid Id { get; set; }
        
    }
}
