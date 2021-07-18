using AutoMapper;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Project;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers.Profiles
{
    public class ProjectRequestProfile : Profile
    {
        
        public ProjectRequestProfile()
        {
            CreateMap<ProjectJson, CreateProjectRequest>()
                    .ForMember(dest => dest.User, opt => opt.Ignore());
            
            CreateMap<ProjectJson, EditProjectRequest>()
                    .ForMember(dest => dest.User, opt => opt.Ignore());
            
            CreateMap<Guid, BaseIdentifyRequest<Guid>>()
                    .ForMember(dest => dest.Id , opt => opt.MapFrom( s => s ))
                    .ForMember(dest => dest.User , opt => opt.Ignore());
        }
    }
}