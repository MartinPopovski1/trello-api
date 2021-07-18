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
    public interface ITicketListHandler
    {
        Task AssignDefaultUser(EditTicketListRequest request);
        Task<TicketList> Create(CreateTicketListRequest request, Guid boardId);
        Task<bool> Delete(BaseIdentifyRequest<Guid> request);
        Task<bool> Edit(EditTicketListRequest request);
        Task<TicketList> Get(BaseIdentifyRequest<Guid> request);
        Task<List<TicketList>> GetAll(BaseIdentifyRequest<Guid> request);
    }
}
