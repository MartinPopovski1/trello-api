using Projector.Core.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Requests
{
    public class BaseIdentifyRequest<TKey> : BaseRequest , IIdentifyResource<TKey>
    {
        public TKey Id { get; set; }
    }
}
