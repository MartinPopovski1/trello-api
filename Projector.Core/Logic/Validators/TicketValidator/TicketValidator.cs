using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Handlers.TicketHandler;
using Projector.Core.Logic.Requests;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Validators.TicketValidator
{
    public class TicketValidator : BaseValidator , ITicketValidator
    {
        ITicketRepository _repository;
        public TicketValidator(ITicketRepository repository)
        {
            _repository = repository;
        }
        public async Task<ValidationResult> ValidateSwitch(BaseIdentifyRequest<Guid> ticketListId , BaseIdentifyRequest<Guid> ticketId)
        {
            
            var ticket = await _repository.GetById(ticketId);
            var nameTaken = await _repository.NameTaken(ticketListId.Id, ticket.Name);
            if(nameTaken)
            {
                var validationResult = new ValidationResult("Cannot Move Ticket !", 403);
                validationResult.AddErrorMessage("The Target Ticket List Already Contains a Ticket by That Name !");
                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }

            return new ValidationResult();
        }
    }
}