using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Models;
using Projector_Web_Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.Mappers
{
    public interface IMapJsonToDomain<TSource, TKey>
    {
        ICreateResource MapCreate(TSource source, User user);
        IEditResource MapEdit(TSource source, User user);
        BaseIdentifyRequest<TKey> MapGet(TKey key); 
    }

}

