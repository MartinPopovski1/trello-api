using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector_Web_Api.ApiModels
{
    public class PrivilegeJson : BaseJsonModel
    {
        public string Name { get; set; }
        public int PrivilegeType { get; set; }
    }
}