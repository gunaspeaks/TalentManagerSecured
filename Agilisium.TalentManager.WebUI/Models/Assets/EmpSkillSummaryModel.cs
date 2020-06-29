using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpSkillSummaryModel : ViewModelBase
    {
        public int EmployeeEntryID { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Limited Skills")] 
        public string LimitedSkills { get; set; }

        [DisplayName("Basic Skills")]
        public string BasicSkills { get; set; }

        [DisplayName("Proficient Skills")]
        public string ProfSkills { get; set; }

        [DisplayName("Advanced Skills")]
        public string AdvanceSkills { get; set; }

        [DisplayName("Expert Skills")]
        public string ExpertSkills { get; set; }
    }
}