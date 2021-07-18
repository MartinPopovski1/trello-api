using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector_Web_Api.Mediatr.Requests
{
    public interface IBaseRequest
    {

    }

    public interface IRequest : IBaseRequest
    {

    }

    public interface IRequest<T> : IBaseRequest
    {

    }

    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}