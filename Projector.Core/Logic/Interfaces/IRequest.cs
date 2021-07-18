using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Interfaces
{
    public interface IRequest
    {
        User User { get; set; }
    }
}
