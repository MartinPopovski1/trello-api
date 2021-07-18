using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.TicketListHandler
{
    public interface ITicketListValidator : IValidator
    {
        Task<ValidationResult> ValidateCreate(Guid boardId, string name);
        Task<ValidationResult> ValidateDelete(Guid ticketListId);
        Task<ValidationResult> ValidateEdit(EditTicketListRequest request);
    }
}