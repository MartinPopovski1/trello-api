using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Project;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.ProjectHandler
{
    public interface IProjectHandler
    {
        Task<Project> Create(CreateProjectRequest request);
        Task Delete(BaseIdentifyRequest<Guid> request);
        Task Edit(EditProjectRequest request);
        Task<Project> Get(BaseIdentifyRequest<Guid> request);
        Task<List<Project>> GetAll();
        Task<List<Project>> Search(string name);
    }
}
