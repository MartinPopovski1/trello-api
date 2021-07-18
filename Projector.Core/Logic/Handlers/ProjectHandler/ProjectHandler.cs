using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Project;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.ProjectHandler
{
    public class ProjectHandler : IProjectHandler
    {
        IProjectRepository _repository;
        IProjectValidator _validator;
        public ProjectHandler(IProjectRepository repository, IProjectValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<List<Project>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Project> Get(BaseIdentifyRequest<Guid> request)
        {
            return await _repository.Get(request.Id);
        }

        public async Task<Project> Create(CreateProjectRequest request)
        {
            var validationResult = await _validator.ValidateCreate(request.Name);
            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);

            var project = new Project(request);
            return await _repository.AddProject(project);
        }

        public async Task Delete(BaseIdentifyRequest<Guid> request)
        {
            var validationResult = await _validator.ValidateDelete(request.Id);
            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);
            await _repository.Delete(request.Id);
        }

        public async Task Edit(EditProjectRequest request)
        {
            var validationResult = await _validator.ValidateEdit(request.Name);
            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);
            await _repository.Edit(request.Id , request.Name);
        }

        
        public async Task<List<Project>> Search(string name)
        {
            return await _repository.Search(name);
        }
    }
}