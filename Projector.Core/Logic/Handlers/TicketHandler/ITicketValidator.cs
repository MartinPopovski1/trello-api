using Projector.Core.Logic.Requests;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.TicketHandler
{
    public interface ITicketValidator
    {
        Task<ValidationResult> ValidateSwitch(BaseIdentifyRequest<Guid> ticketListId, BaseIdentifyRequest<Guid> ticketId);
    }
}