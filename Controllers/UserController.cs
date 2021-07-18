using AutoMapper;
using Projector.Core.Logic.Handlers.UserHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Users;
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

    public class UserController : BaseApiController
    {

        IUserHandler _handler;
        IMapJsonToDomain<UserJson, Guid> _mapToDomain;
        IMapDomainToJson<User, UserJson> _mapToJson;
        IMapDomainToJson<Privilege, PrivilegeJson> _mapPrivileges;
        IMapper _mapper;

        public UserController(IUserHandler handler, IUserService userService , IMapJsonToDomain<UserJson,Guid> mapToDomain , 
                                    IMapDomainToJson<Privilege,PrivilegeJson> mapPrivileges , IMapDomainToJson<User,UserJson> mapToJson) : base(userService)
        {
            _handler = handler;
            _mapToDomain = mapToDomain;
            _mapToJson = mapToJson;
            _mapPrivileges = mapPrivileges;
        }

        [HttpGet]
        [Route("api/user")]
        public async Task<IHttpActionResult> GetAll()
        {

            var result = await _handler.GetAll();
            return Ok(_mapToJson.MapToListJson(result));

        }

        [HttpGet]
        [Route("api/user/{userId}")]
        public async Task<IHttpActionResult> GetById([FromUri] Guid userId)
        {
            var request = _mapToDomain.MapGet(userId) as BaseIdentifyRequest<Guid>;
            var result = await _handler.FindUser(request);
            return Ok(_mapToJson.MapToJson(result));

        }

        [HttpPost]
        [Route("api/user")]
        public async Task<IHttpActionResult> CreateUser([FromBody] UserJson userModel, [FromBody] string password)
        {
            var request = _mapToDomain.MapCreate(userModel, User) as CreateUserRequest;

            await _handler.CreateUser(request, password);

            return Ok();
        }

        [HttpDelete]
        [Route("api/user")]
        public async Task<IHttpActionResult> DeleteUser([FromUri] Guid userId)
        {
            var request = _mapToDomain.MapGet(userId) as BaseIdentifyRequest<Guid>;

            await _handler.RemoveUser(request);

            return Ok();
        }

        [HttpPatch]
        [Route("api/user/{userId}/changePassword")]
        public async Task<IHttpActionResult> ChangePassword([FromUri] Guid userId, [FromBody] string oldPassword, [FromBody] string newPassword)
        {
            var request = _mapToDomain.MapGet(userId) as BaseIdentifyRequest<Guid>;

            var accessToken = await _handler.ChangePassword(request, oldPassword, newPassword);
            
            return Ok(accessToken);

        }
        [HttpPatch]
        [Route("api/user/{userId}/setPrivileges")]
        public async Task<IHttpActionResult> SetPrivileges([FromUri] Guid userId, [FromBody] UserJson userJson)
        {
            var user = _mapper.Map<User>(userJson);
            user.Id = userId;

            await _handler.SetPrivileges(user);

            return Ok();
        }

        [HttpPatch]
        [Route("api/user/{userId}/updateUserInfo")]
        public async Task<IHttpActionResult> Update([FromUri] Guid userId, [FromBody]UserJson userJson)
        {
            var request = _mapToDomain.MapEdit(userJson, User) as EditUserRequest;
            request.Id = userId;

            var user = await _handler.UpdateInfo(request);

            return Ok(_mapToJson.MapToJson(user));
        }

        [HttpGet]
        [Route("api/user/{userId}/getUserPrivileges")]
        public async Task<IHttpActionResult> GetUserPrivileges([FromUri] Guid userId)
        {
            var request = _mapToDomain.MapGet(userId) as BaseIdentifyRequest<Guid>;
            var privileges = await _handler.GetUserPrivileges(request);

            return Ok(_mapPrivileges.MapToListJson(privileges));
        }
    }
}