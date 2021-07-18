using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Handlers.BoardHandler;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Validators.BoardValidators
{

    public class BoardValidator : BaseValidator, IBoardValidator
    {
        IBoardRepository _repository;
        public BoardValidator(IBoardRepository repository) : base()
        {
            _repository = repository;
            ThrowException = true;
        }

        public async Task<ValidationResult> ValidateCreate(Guid projectId, string name)
        {
            var result = await _repository.NameTaken(projectId, name);
            if(result)
            {
                var validationResult = new ValidationResult("Cannot Create Board !", 403);
                validationResult.AddErrorMessage("Board Name is Already Taken !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }
            return new ValidationResult();
        }

        public async Task<ValidationResult> ValidateDelete(Guid id)
        {
            var result  = await _repository.HasActiveTicketLists(id);

            if(result)
            {
                var validationResult = new ValidationResult("Cannot Delete Board !", 403);
                validationResult.AddErrorMessage("Selected Board Has Active Ticket Lists !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> ValidateEdit(Guid projectId, string name)
        {
            var result = await _repository.NameTaken(projectId, name);
            if (result)
            {
                var validationResult = new ValidationResult("Cannot Create Board !", 403);
                validationResult.AddErrorMessage("Board Name is Already Taken !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }
            return new ValidationResult();
        }
    }
}