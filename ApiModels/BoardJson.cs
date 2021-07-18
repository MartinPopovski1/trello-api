using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.ApiModels
{
    public class BoardJson : BaseJsonModel
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ProjectId { get; set; }
        public List<TicketListJson> TicketLists { get; set; } = null;
    }
}