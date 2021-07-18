using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Handlers.UserHandler;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Validators.UserValidators
{
    public class UserValidator : BaseValidator , IUserValidator
    {
        IUserRepository _repository;
        public UserValidator(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ValidationResult> ValidateUserName(string userName)
        {
            var userNameTaken = await _repository.UserNameTaken(userName);

            if(userNameTaken)
            {
                var validationResult = new ValidationResult("User Name is Already Taken !" , 403);

                if (ThrowException)
                {
                    throw new BusinessException(validationResult);
                }
                else
                    return validationResult;
            }

            return new ValidationResult();
        }

        public ValidationResult ValidatePassword(string password)
        {
            if(password.Length < 8)
            {
                var validationResult = new ValidationResult("Password cannot be less than 8 characters !", 403);

                if (ThrowException)
                    throw new BusinessException(validationResult);
                else
                    return validationResult;
            }
            // MORE VALIDATION TO BE ADDED HERE
            return new ValidationResult();
        }
    }
}
