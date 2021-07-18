using AutoMapper;
using Castle.Core.Internal;
using Projector.Core.Logic.Handlers.TicketListHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using Projector_Web_Api.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Projector_Web_Api.Controllers
{
    [RoutePrefix("api/projects/{projectId}/boards/{boardId}/ticketLists")]
    public class TicketListController : BaseApiController
    {
        ITicketListHandler _handler;
        IMapJsonToDomain<TicketListJson,Guid> _jsonToDomainMapper;
        IMapDomainToJson<TicketList, TicketListJson> _domainToJsonMapper;
        public TicketListController(ITicketListHandler handler, IUserService userService, IMapJsonToDomain<TicketListJson, Guid> iMapDomainToJson, 
                                        IMapDomainToJson<TicketList, TicketListJson> domainToJsonMapper) : base(userService)
        {
            _handler = handler;
            _jsonToDomainMapper = iMapDomainToJson;
            _domainToJsonMapper = domainToJsonMapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll([FromUri]Guid boardId)
        {
            var request = _jsonToDomainMapper.MapGet(boardId) as BaseIdentifyRequest<Guid>;
            var result = await _handler.GetAll(request);
            return Ok(_domainToJsonMapper.MapToListJson(result));

        }
        [HttpGet]
        [Route("{ticketListId}")]
        public async Task<IHttpActionResult> GetById([FromUri] Guid ticketListId)
        {

            var result = await _handler.Get(_jsonToDomainMapper.MapGet(ticketListId) as BaseIdentifyRequest<Guid>);
            return Ok(_domainToJsonMapper.MapToJson(result));

        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddTicketList([FromBody] TicketListJson model, [FromUri] Guid boardId)
        {
            if(model.Name.IsNullOrEmpty())
            {
                return BadRequest("Name Cannot be Empty !");
            }

            var request = _jsonToDomainMapper.MapCreate(model, User) as CreateTicketListRequest;
            var ticketList = await _handler.Create(request, boardId);

            return Ok(_domainToJsonMapper.MapToJson(ticketList));

        }
        [HttpDelete]
        [Route("{ticketListId}")]
        public async Task<IHttpActionResult> Delete([FromUri] Guid ticketListId)
        {

            await _handler.Delete(_jsonToDomainMapper.MapGet(ticketListId) as BaseIdentifyRequest<Guid>);

            return Ok();

        }
        [HttpPatch]
        [Route("{ticketListId}")]
        public async Task<IHttpActionResult> EditTicketList([FromBody] TicketListJson model, [FromUri] Guid ticketListId)
        {
            if(model.Name.IsNullOrEmpty())
            {
                return BadRequest("Name Cannot be Empty !");
            }

            var request = _jsonToDomainMapper.MapEdit(model, User) as EditTicketListRequest;
            request.Id = ticketListId;
            await _handler.Edit(request);

            return Ok();

        }
    }
}