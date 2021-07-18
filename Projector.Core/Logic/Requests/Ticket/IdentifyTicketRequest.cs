using Projector.Core.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests.Ticket
{
    public class IdentifyTicketRequest : BaseIdentifyRequest<Guid> , IIdentifyResource<Guid>
    {
        public Guid Id { get; set; }
    }
}
