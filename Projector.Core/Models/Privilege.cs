using Projector.Core.Models.Enums;

namespace Projector.Core.Models
{
    public class Privilege : BaseModel
    {

        public string Name { get; set; }
        public PrivilegeEnum PrivilegeType { get; set; }

    }
}