using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class EmployeeLoginMapping : EntityBase
    {
        [Key]
        public int MappingID { get; set; }

        public int EmployeeID { get; set; }

        public string LoginUserID { get; set; }

        public bool IsBlocked { get; set; }

        public string RoleID { get; set; }
    }
}
