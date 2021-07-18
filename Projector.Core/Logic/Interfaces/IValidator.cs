using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Interfaces
{
    public interface IValidator
    {
        bool ThrowException { get; set; }
    }
}