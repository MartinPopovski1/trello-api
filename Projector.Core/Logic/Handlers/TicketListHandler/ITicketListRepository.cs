using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.TicketListHandler
{
    public interface ITicketListRepository
    {
        Task AssignDefaultUser(Guid ticketListId, Guid userId);
        Task CreateTicketList(TicketList ticketList);
        Task Delete(BaseIdentifyRequest<Guid> request);
        Task EditTicketList(EditTicketListRequest request);
        Task<List<TicketList>> GetAll(BaseIdentifyRequest<Guid> request);
        Task<TicketList> GetById(Guid ticketListId);
        Task<bool> HasActiveTickets(Guid ticketListId);
        Task<bool> NameTaken(Guid boardId, string name);
    }
}