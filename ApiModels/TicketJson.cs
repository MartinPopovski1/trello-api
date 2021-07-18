using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.ApiModels
{
    public class TicketJson : BaseJsonModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid TicketListId { get; set; }

    }
}