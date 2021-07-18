using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Ticket;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.TicketHandler
{
    public interface ITicketHandler
    {
        Task<Ticket> Create(CreateTicketRequest request, Guid ticketListId);
        Task Delete(BaseIdentifyRequest<Guid> request);
        Task Edit(EditTicketRequest request);
        Task SwitchTicket(BaseIdentifyRequest<Guid> ticketListId, BaseIdentifyRequest<Guid> ticketId);
        Task AssignUsers(Guid ticketId, List<Guid> userId);
        Task RemoveUsersFromTicket(Guid ticketId, List<Guid> userId);
    }
}
