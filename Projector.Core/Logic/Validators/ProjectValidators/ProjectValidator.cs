using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Handlers.ProjectHandler;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Validators.ProjectValidators
{

    public class ProjectValidator : BaseValidator,IProjectValidator
    {
        IProjectRepository _repository;
        public ProjectValidator(IProjectRepository repository) : base()
        {
            _repository = repository;
        }

        public async Task<ValidationResult> ValidateCreate(string name)
        {
            var nameTaken =  await _repository.NameTaken(name);

            if(nameTaken)
            {
                var validationResult = new ValidationResult("Cannot Create Project !", 403);
                validationResult.AddErrorMessage("The Name is Alreay Taken !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }

            return new ValidationResult();
        }

        public async Task<ValidationResult> ValidateDelete(Guid id)
        {
            var hasActiveBoards = await _repository.HasActiveBoards(id);

            if(hasActiveBoards)
            {
                var validationResult = new ValidationResult("Cannot Delete Project !", 403);
                validationResult.AddErrorMessage("The Project Has Active Boards !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }
            return new ValidationResult();
        }

        public async Task<ValidationResult> ValidateEdit(string name)
        {
            var nameTaken = await _repository.NameTaken(name);

            if (nameTaken)
            {
                var validationResult = new ValidationResult("Cannot Edit Project !", 403);
                validationResult.AddErrorMessage("The Name is Alreay Taken !");

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }

            return new ValidationResult();
        }
    }
}