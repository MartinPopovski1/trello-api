using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests.Project;
using System;
using System.Collections.Generic;

namespace Projector.Core.Models
{
    public class Project : BaseModel, IRecordHistory
    {
        public Project()
        {
            Boards = new List<Board>();
        }

        public Project(CreateProjectRequest request)
        {
            Id = Guid.NewGuid();
            Name = request.Name;
            CreatedAt = DateTime.Now;
            Deleted = false;
            //CreateBy = request.User.Id;
            Boards = new List<Board>();
        }
        public string Name { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedAt { get; set; }
        //public Guid CreateBy { get; set; }
        public List<Board> Boards { get; set; }
    }
}