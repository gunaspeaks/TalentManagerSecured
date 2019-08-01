
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class UserRoleDto : DtoBase
    {
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
