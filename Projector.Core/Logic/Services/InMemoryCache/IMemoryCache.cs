using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Logic.Services.InMemoryCache
{
    public interface IMemoryCache
    {
        object GetValue(string key);

        bool Add(string key, object value, DateTimeOffset absExpiration);

        void Delete(string key);
    }
}   