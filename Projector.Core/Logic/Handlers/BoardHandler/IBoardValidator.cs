using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.BoardHandler
{
    public interface IBoardValidator : IValidator
    {
        Task<ValidationResult> ValidateCreate(Guid projectId, string name);
        Task<ValidationResult> ValidateEdit(Guid projectId, string name);
        Task<ValidationResult> ValidateDelete(Guid id);
        
    }
}