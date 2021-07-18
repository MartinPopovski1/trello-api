using AutoMapper;
using Projector.Core.Logic.Handlers.BoardHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Board;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using Projector_Web_Api.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Projector_Web_Api.Controllers
{
    [RoutePrefix("api/projects/{projectId}/boards")]
    public class BoardController : BaseApiController
    {
        IBoardHandler _handler;
        IMapJsonToDomain<BoardJson,Guid> _jsonToDomainMapper;
        IMapDomainToJson<Board, BoardJson> _domainToJsonMapper;
        public BoardController(IBoardHandler handler, IUserService userService, IMapJsonToDomain<BoardJson, Guid> iMapJsonToDomain, IMapDomainToJson<Board, BoardJson> domainToJsonMapper) : base(userService)
        {
            _handler = handler;
            _jsonToDomainMapper = iMapJsonToDomain;
            _domainToJsonMapper = domainToJsonMapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll([FromUri] Guid projectId)
        {

            var result = await _handler.GetAll(projectId);

            return Ok(_domainToJsonMapper.MapToListJson(result));

        }

        [HttpGet]
        [Route("{boardId}")]
        public async Task<IHttpActionResult> Get([FromUri]Guid boardId)
        {

            var result = await _handler.Get(_jsonToDomainMapper.MapGet(boardId) as BaseIdentifyRequest<Guid>);

            return Ok(_domainToJsonMapper.MapToJson(result));

        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddBoard([FromUri] Guid projectId, [FromBody] BoardJson model)
        {

            var request = _jsonToDomainMapper.MapCreate(model, User) as CreateBoardRequest;
            
            var board = await _handler.Create(request, projectId);

            return Ok(_domainToJsonMapper.MapToJson(board));

        }

        [HttpPut]
        [Route("{boardId}")]
        public async Task<IHttpActionResult> EditBoard([FromUri] Guid boardId, [FromBody] BoardJson model, [FromUri] Guid projectId)
        {
            if (string.IsNullOrEmpty(model.Name))
                return BadRequest("Board name cannot be empty");

            var request = _jsonToDomainMapper.MapEdit(model, User) as EditBoardRequest;
            request.Id = boardId;

            await _handler.Edit(request,projectId);
            
            return Ok();
        }

        [HttpDelete]
        [Route("{boardId}")]
        public async Task<IHttpActionResult> DeleteBoard([FromUri] Guid boardId)
        {

            await _handler.Delete(_jsonToDomainMapper.MapGet(boardId) as BaseIdentifyRequest<Guid>);

            return Ok();

        }

        [HttpGet]
        [Route("search/{name}")]
        public async Task<IHttpActionResult> Search([FromUri] Guid projectId, [FromUri] string name)
        {

            var result = await _handler.Search(projectId, name);

            return Ok(_domainToJsonMapper.MapToListJson(result));

        }
    }
}