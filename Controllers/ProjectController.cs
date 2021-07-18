using AutoMapper;
using Ninject;
using Projector.Core.Logic.Handlers.ProjectHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Project;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using Projector_Web_Api.Filters;
using Projector_Web_Api.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Projector_Web_Api.Controllers
{
    
    public class ProjectController : BaseApiController
    {
        IProjectHandler _handler;
        IMapJsonToDomain<ProjectJson,Guid> _jsonToDomainMapper;        
        IMapDomainToJson<Project, ProjectJson> _domainToJsonMapper;

        public ProjectController(IProjectHandler handler, IUserService userService, IMapJsonToDomain<ProjectJson,Guid> iMapJsonToDomain ,
                                    IMapDomainToJson<Project,ProjectJson> iMapDomainToJson) : base(userService)
        {
            _handler = handler;
            _jsonToDomainMapper = iMapJsonToDomain;
            _domainToJsonMapper = iMapDomainToJson;
        }

        [HttpPost]
        [Route("api/projects")]
        public async Task<IHttpActionResult> AddProject([FromBody] ProjectJson model)
        {

            var createRequest = _jsonToDomainMapper.MapCreate(model, User) as CreateProjectRequest;

            var project = await _handler.Create(createRequest);

            return Ok(_domainToJsonMapper.MapToJson(project));

        }

        [HttpGet]
        [Route("api/projects")]
        public async Task<IHttpActionResult> GetAll()
        {

            var projects = await _handler.GetAll();

            return Ok(_domainToJsonMapper.MapToListJson(projects));

        }

        [HttpGet]
        [Route("api/projects/{projectId}")]
        public async Task<IHttpActionResult> Get([FromUri] Guid projectId)
        {
            
            var request = _jsonToDomainMapper.MapGet(projectId) as BaseIdentifyRequest<Guid>;
            var project = await _handler.Get(request);

            return Ok(_domainToJsonMapper.MapToJson(project));

        }

        [HttpDelete]
        [Route("api/projects/{projectId}")]
        public async Task<IHttpActionResult> Delete([FromUri] Guid projectId)
        {

            var request = _jsonToDomainMapper.MapGet(projectId) as BaseIdentifyRequest<Guid>;

            await _handler.Delete(request);

            return Ok();

        }

        [HttpPut]
        [Route("api/projects/{projectId}")]
        public async Task<IHttpActionResult> Edit([FromUri] Guid projectId, [FromBody] ProjectJson project)
        {
            if (string.IsNullOrEmpty(project.Name))
                return BadRequest("Name must be provided");

            var request = _jsonToDomainMapper.MapEdit(project, User) as EditProjectRequest;
            request.Id = projectId;

            await _handler.Edit(request);

            var result = await _handler.Get(_jsonToDomainMapper.MapGet(projectId) as BaseIdentifyRequest<Guid>);
            return Ok(_domainToJsonMapper.MapToJson(result));
        }

        [HttpGet]
        [Route("api/projects/search/{name}")]
        public async Task<IHttpActionResult> Search([FromUri] string name)
        {

            if (String.IsNullOrEmpty(name))
                return BadRequest("Search parameter cannot be emptry");

            var result = await _handler.Search(name);

            return Ok(_domainToJsonMapper.MapToListJson(result));

        }
    }
}