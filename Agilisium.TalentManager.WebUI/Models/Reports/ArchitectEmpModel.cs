using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class ArchitectEmpModel
    {
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }

        [DisplayName("Account")]
        public string AccountName { get; set; }

        [DisplayName("Project")]
        public string ProjectName { get; set; }

        [DisplayName("Allocated From")]
        public DateTime? AllocatedFrom { get; set; }

        [DisplayName("Allocated Upto")]
        public DateTime? AllocatedUpTo { get; set; }

        [DisplayName("Allocation Type")]
        public string AllocationType { get; set; }

    }
}