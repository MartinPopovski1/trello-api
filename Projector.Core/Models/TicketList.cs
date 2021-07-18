using Projector.Core.Logic.Requests.TicketList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Models
{
    public class TicketList : BaseModel
    {
        public TicketList(CreateTicketListRequest request ,Guid boardId)
        {
            Id = Guid.NewGuid();
            Name = request.Name;
            CreatedAt = DateTime.Now;
            Deleted = false;
            BoardId = boardId;
            Tickets = new List<Ticket>();
            DefaultAssignedUser = request.DefaultAssignedUser;
        }
        public TicketList()
        {
            Tickets = new List<Ticket>();
        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Ticket> Tickets { get; set; }
        public Guid BoardId { get; set; }
        public bool Deleted { get; set; }
        public Guid DefaultAssignedUser { get; set; }
    }
}