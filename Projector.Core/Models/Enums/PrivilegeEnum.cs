using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projector.Core.Models.Enums
{
    public enum PrivilegeEnum
    {
        AddProject = 0,
        EditProject = 1,
        DeleteProject = 2,        

        AddBoard = 3,
        DeleteBoard = 4,
        EditBoard = 5,

        CreateTicket= 6,
        EditTicket = 7,
        FinishTicket = 8,

        CreateTicketList = 9,
        EditTicketList = 10,
        DeleteTicketList =  11,        
        // ...
    }
}