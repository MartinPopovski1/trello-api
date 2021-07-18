using AutoMapper;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers
{
    public class GenericJsonToDomainMapper<TSource,TKey,TCreateRequest,TEditRequest,TIdentifyRequest> :  IMapJsonToDomain<TSource,TKey> 
                                                                              where TSource : BaseJsonModel
                                                                              where TCreateRequest : ICreateResource
                                                                              where TEditRequest : IEditResource
                                                                              where TIdentifyRequest : BaseIdentifyRequest<TKey>
    {
        private IMapper _mapper;

        public GenericJsonToDomainMapper(IMapper mapper)
        {
            _mapper = mapper;
            
            
        }

        public ICreateResource MapCreate(TSource source, User user)
        {
            var createRequest = _mapper.Map<TCreateRequest>(source);
            createRequest.User = user;
            
            return createRequest;
        }
        
        public IEditResource MapEdit(TSource source, User user)
        {
            var editRequest = _mapper.Map<TEditRequest>(source);
            editRequest.User = user;
            return editRequest;
        }

        public BaseIdentifyRequest<TKey> MapGet(TKey key)
        {
            var request = _mapper.Map<BaseIdentifyRequest<TKey>>(key);
 
            
            return request;
        }
    }
}