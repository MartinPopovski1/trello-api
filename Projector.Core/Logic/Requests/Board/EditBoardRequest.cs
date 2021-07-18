using Projector.Core.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests.Board
{
    public class EditBoardRequest : BaseRequest , IEditResource
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
