using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.ProjectHandler
{
    public interface IProjectValidator : IValidator
    {
        Task<ValidationResult> ValidateCreate(string name);
        Task<ValidationResult> ValidateDelete(Guid id);
        Task<ValidationResult> ValidateEdit(string name);
    }
}