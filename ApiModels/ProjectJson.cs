using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projector_Web_Api.ApiModels
{
    public class ProjectJson : BaseJsonModel
    {
        
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<BoardJson> Boards { get; set; } = null;

    }
}