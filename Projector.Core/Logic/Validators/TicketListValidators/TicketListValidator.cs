using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Handlers.TicketListHandler;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Validators.TicketListValidators
{
    public class TicketListValidator : BaseValidator , ITicketListValidator
    {
        ITicketListRepository _repository;
        public TicketListValidator(ITicketListRepository repository)
        {
            _repository = repository;
        }
        public async Task<ValidationResult> ValidateDelete(Guid ticketListId)
        {
            var hasActiveTickets = await _repository.HasActiveTickets(ticketListId);

            if (hasActiveTickets)
            {
                var validationResult = new ValidationResult("Cannot Delete Ticket List !", 403);
                validationResult.AddErrorMessage("Selected Ticket List Has Active Tickets !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }

            return new ValidationResult();

        }

        public async Task<ValidationResult> ValidateCreate(Guid boardId, string name)
        {
            var nameTaken = await _repository.NameTaken(boardId, name);

            if(nameTaken)
            {
                var validationResult = new ValidationResult("Cannot Create Ticket List !", 403);
                validationResult.AddErrorMessage("The Name is Already Taken !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else return validationResult;
            }

            return new ValidationResult();
        }
        
        public async Task<ValidationResult> ValidateEdit(EditTicketListRequest request)
        {
            var nameTaken = await _repository.NameTaken(request.BoardId, request.Name);

            if(nameTaken)
            {
                var validationResult = new ValidationResult("Cannot Edit Ticket List !", 403);
                validationResult.AddErrorMessage("The Name is Already Taken !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else return validationResult;
            }

            return new ValidationResult();
        }
    }
}