using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers
{
    public interface IMapDomainToJson<TSource,TResponse> where TResponse : BaseJsonModel where TSource : BaseModel
    {
        TResponse MapToJson(TSource source);
        IEnumerable<TResponse> MapToListJson(IEnumerable<TSource> source);
    }
}