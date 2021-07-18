
using Projector.Core.Logic.Requests.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Models
{
    public class Board : BaseModel
    {
        public Board(CreateBoardRequest request , Guid projectId)
        {
            Id = Guid.NewGuid();
            Name = request.Name;
            CreatedAt = DateTime.Now;
            Deleted = false;
            TicketLists = new List<TicketList>();
            ProjectId = projectId;
        }

        public Board()
        {
            TicketLists = new List<TicketList>();
        }

        public string Name { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ProjectId { get; set; }
        public List<TicketList> TicketLists { get; set; }
    }
}