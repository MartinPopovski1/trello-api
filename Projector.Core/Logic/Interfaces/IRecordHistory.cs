using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Logic.Interfaces
{
    public interface IRecordHistory
    {
        DateTime CreatedAt { get; set; }
        //Guid CreateBy { get; set; }
        //Guid LastModifiedBy { get; set; }
        //DateTime LastEditedAt { get; set; }
    }
}