using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Models
{
    public class UserTickets : BaseModel
    { 
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
    }
}
