using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmployeeLoginMappingModel : ViewModelBase
    {
        public int MappingID { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage ="Please select an Employee")]
        public int EmployeeID { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Portal User")]
        [Required(ErrorMessage = "Please select an Portal User")]
        public string LoginUserID { get; set; }

        [DisplayName("Email ID")]
        public string LoginUserEmail { get; set; }

        [DisplayName("Role Name")]
        [Required(ErrorMessage = "Please select a Role")]
        public string RoleID { get; set; }

        [DisplayName("Role Name")]
        public string RoleName { get; set; }

        [DisplayName("Is Blocked")]
        public bool IsBlocked { get; set; }
    }
}