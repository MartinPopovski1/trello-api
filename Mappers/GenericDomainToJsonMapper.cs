using AutoMapper;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers
{
    public class GenericDomainToJsonMapper<TSource, TResult> :  IMapDomainToJson<TSource, TResult> where TSource : BaseModel where TResult : BaseJsonModel
    {
        IMapper _mapper;
        public GenericDomainToJsonMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public TResult MapToJson(TSource source)
        {
            var result = _mapper.Map<TResult>(source);
            return result;
        }
        
        public IEnumerable<TResult> MapToListJson(IEnumerable<TSource> source)
        {
            var result = _mapper.Map<IEnumerable<TResult>>(source);
            return result;
        }
    
    }
}