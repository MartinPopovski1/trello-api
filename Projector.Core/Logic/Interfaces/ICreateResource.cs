using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Interfaces
{
    public interface ICreateResource : IRequest
    {
        string Name { get; set; }
    }
}
