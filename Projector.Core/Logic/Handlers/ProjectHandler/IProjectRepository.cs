using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.ProjectHandler
{
    public interface IProjectRepository
    {
        Task<Project> AddProject(Project p);
        Task Delete(Guid projectId);
        Task Edit(Guid projectId, string Name);
        Task<Project> Get(Guid id);
        Task<List<Project>> GetAll();
        Task<bool> NameTaken(string name);
        Task<bool> HasActiveBoards(Guid id);
        Task<List<Project>> Search(string name);
    }
}