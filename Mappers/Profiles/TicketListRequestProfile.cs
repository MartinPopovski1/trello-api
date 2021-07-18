using AutoMapper;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.TicketList;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers.Profiles
{
    public class TicketListRequestProfile : Profile
    {
        public TicketListRequestProfile()
        {
            CreateMap<TicketListJson, CreateTicketListRequest>().ForMember(dest => dest.User , opt => opt.Ignore());
            CreateMap<TicketListJson, EditTicketListRequest>().ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Guid, BaseIdentifyRequest<Guid>>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
        }
    }
}