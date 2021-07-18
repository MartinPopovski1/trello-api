using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.TicketListHandler
{
    public class TicketListHandler : ITicketListHandler
    {
        ITicketListRepository _repository;
        ITicketListValidator _validator;
        public TicketListHandler(ITicketListRepository repository, ITicketListValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<List<TicketList>> GetAll(BaseIdentifyRequest<Guid> request)
        {
            return await _repository.GetAll(request);
        }

        public async Task<TicketList> Get(BaseIdentifyRequest<Guid> request)
        {
            return await _repository.GetById(request.Id);
        }

        public async Task<TicketList> Create(CreateTicketListRequest request, Guid boardId)
        {
            
            var ticketList = new TicketList(request, boardId);
            await _repository.CreateTicketList(ticketList);
            return ticketList;
        }
        public async Task<bool> Edit(EditTicketListRequest request)
        {
            if (!(await _validator.ValidateEdit(request)).IsSuccessful)
                return false;

            await _repository.EditTicketList(request);

            return true;
        }

        public async Task<bool> Delete(BaseIdentifyRequest<Guid> request)
        {
            if (!(await _validator.ValidateDelete(request.Id)).IsSuccessful)
                return false;
            await _repository.Delete(request);
            return true;
        }
        public async Task AssignDefaultUser(EditTicketListRequest request)
        {
            await _repository.AssignDefaultUser(request.Id, request.DefaultAssignedUser);
        }
    }
}
