using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Ticket;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.TicketHandler
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateTicket(Ticket ticket);
        Task Delete(BaseIdentifyRequest<Guid> request);
        Task EditTicket(EditTicketRequest   request);
        Task<Ticket> GetById(BaseIdentifyRequest<Guid> request);
        Task<bool> NameTaken(Guid ticketListId, string name);
        Task SetDescription(Ticket ticket, Guid ticketId);
        Task AssignUsers(Guid ticketId, List<Guid> userId);
        Task RemoveUsersFromTicket(Guid ticketId, List<Guid> userId);
        Task ChangeParrentTicketList(Guid ticketListId, Guid ticketId);
    }
}