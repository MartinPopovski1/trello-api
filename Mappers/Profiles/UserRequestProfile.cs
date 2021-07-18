using AutoMapper;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Users;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;

namespace Projector_Web_Api.Mappers.Profiles
{
    public class UserRequestProfile : Profile
    {
        public UserRequestProfile()
        {
            CreateMap<UserJson, CreateUserRequest>();
            CreateMap<UserJson, EditUserRequest>();
            CreateMap<Guid, BaseIdentifyRequest<Guid>>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
        }
    }
}