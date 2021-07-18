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
    public class TicketHandler : ITicketHandler
    {
        ITicketRepository _repository;
        ITicketValidator _validator;
        public TicketHandler(ITicketRepository repository, ITicketValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async  Task AssignUsers(Guid ticketId, List<Guid> userId)
        {
            await _repository.AssignUsers(ticketId, userId);
        }

        public async Task<Ticket> Create(CreateTicketRequest request, Guid ticketListId)
        {
            var ticket = new Ticket(request, ticketListId);
            return await _repository.CreateTicket(ticket);
        }
        public async Task Delete(BaseIdentifyRequest<Guid> request)
        {
            await _repository.Delete(request);
        }

        public async Task Edit(EditTicketRequest request)
        {
            await _repository.EditTicket(request);
        }

        public async Task SwitchTicket(BaseIdentifyRequest<Guid> ticketListId, BaseIdentifyRequest<Guid> ticketId)
        {
            

            if ((await _validator.ValidateSwitch(ticketListId, ticketId)).IsSuccessful)
                await _repository.ChangeParrentTicketList(ticketListId.Id, ticketId.Id);
        }

        public async Task RemoveUsersFromTicket(Guid ticketId, List<Guid> userId)
        {
            await _repository.RemoveUsersFromTicket(ticketId, userId);
        }
        
    }
}
