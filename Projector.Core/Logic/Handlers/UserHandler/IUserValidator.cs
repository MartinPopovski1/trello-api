using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.UserHandler
{
    public interface IUserValidator
    {
        ValidationResult ValidatePassword(string password);
        Task<ValidationResult> ValidateUserName(string userName);
    }
}
