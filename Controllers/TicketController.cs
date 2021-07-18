using AutoMapper;
using Projector.Core.Logic.Handlers.TicketHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Ticket;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using Projector_Web_Api.Mappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Projector_Web_Api.Controllers
{
    [RoutePrefix("api/projects/{projectId}/boards/{boardId}/ticketLists/{ticketListId}/tickets")]
    public class TicketController : BaseApiController
    {
        ITicketHandler _handler;

        IMapJsonToDomain<TicketJson, Guid> _jsonToDomainMapper;
        IMapDomainToJson<Ticket, TicketJson> _domainToJsonMapper;
        public TicketController(ITicketHandler handler, IUserService userService, IMapJsonToDomain<TicketJson, Guid> iMapDomainToJson,
                                    IMapDomainToJson<Ticket, TicketJson> domainToJsonMapper) : base(userService)
        {
            _handler = handler;
            _jsonToDomainMapper = iMapDomainToJson;
            _domainToJsonMapper = domainToJsonMapper;
        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateTicket([FromBody] TicketJson model, [FromUri] Guid ticketListId)
        {

            var request = _jsonToDomainMapper.MapCreate(model, User) as CreateTicketRequest;

            var result = await _handler.Create(request, ticketListId);
            return Ok(_domainToJsonMapper.MapToJson(result));

        }

        [HttpDelete]
        [Route("{ticketId}")]
        public async Task<IHttpActionResult> Delete([FromUri] Guid ticketId)
        {

            await _handler.Delete(_jsonToDomainMapper.MapGet(ticketId) as BaseIdentifyRequest<Guid>);

            return Ok();

        }
        [HttpPatch]
        [Route("{ticketId}")]
        public async Task<IHttpActionResult> EditTicket([FromBody]TicketJson model, [FromUri] Guid ticketId)
        {

            var request = _jsonToDomainMapper.MapEdit(model, User) as EditTicketRequest;
            request.Id = ticketId;

            await _handler.Edit(request);

            return Ok();

        }

        [HttpPatch]
        [Route("{ticketId}/switch")]
        public async Task<IHttpActionResult> SwitchTicket([FromUri] Guid ticketListId, [FromUri] Guid ticketId)
        {
            var ticketListRequest = _jsonToDomainMapper.MapGet(ticketListId) as BaseIdentifyRequest<Guid>;
            var ticketRequest = _jsonToDomainMapper.MapGet(ticketId) as BaseIdentifyRequest<Guid>;
            await _handler.SwitchTicket(ticketListRequest, ticketRequest);
            return Ok();
        }

        [HttpPatch]
        [Route("{ticketId}/assignUsers")]
        public async Task<IHttpActionResult> AssignUsers([FromUri] Guid ticketId, [FromBody] List<Guid> userIds)
        {
            await _handler.AssignUsers(ticketId, userIds);
            return Ok();
        }

        [HttpPatch]
        [Route("{ticketId}/removeUsers")]
        public async Task<IHttpActionResult> RemoveUsersFromTicket([FromUri] Guid ticketId, [FromBody] List<Guid> userIds)
        {
            await _handler.RemoveUsersFromTicket(ticketId, userIds);
            return Ok();
        }
    }
}