using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.ApiModels
{
    public class TicketListJson : BaseJsonModel
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TicketJson> Tickets { get; set; } = null;
        public Guid BoardID { get; set; }
        public Guid DefaultAssignedUser { get; set; }
    }
}