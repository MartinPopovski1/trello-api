using Projector.Core.Logic.Requests.Ticket;
using Projector.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Models
{
    public class Ticket : BaseModel
    {
        public Ticket(CreateTicketRequest request, Guid ticketListId)
        {
            Id = Guid.NewGuid();
            Name = request.Name;
            Description = request.Description;
            CreatedAt = DateTime.Now;
            Deleted = false;
            TicketListId = ticketListId;
        }
        public Ticket()
        {

        }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public Guid TicketListId { get; set; }
        public bool Deleted { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime FinishTime { get; set; }
        public TicketStateEnum TicketState { get; set; }
    }
}