using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class EmployeeSkillsReportDto : DtoBase
    {
        public int EmployeeEntryID { get; set; }

        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string AccountName { get; set; }

        public int AccountID { get; set; }

        public string ProjectName { get; set; }

        public int ProjectID { get; set; }

        public string AllocationType { get; set; }

        public int AllocationTypeID { get; set; }

        public string TotalExperience { get; set; }

        public string LimitedSkills { get; set; }

        public string BasicSkills { get; set; }

        public string ExpertSkills { get; set; }

        public string AdvancedSkills { get; set; }

        public string ProficientSkills { get; set; }

        public string ProjectManager { get; set; }

        public int ProjectManagerID { get; set; }

        public string ReportingManager { get; set; }

        public int ReportingManagerID { get; set; }

        public DateTime? LastWorkingDay { get; set; }

        public DateTime? AlloctionStartDate { get; set; }

        public DateTime? AllocationEndDate { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public float? PastExperience { get; set; }

        public string OverallExperience { get; set; }

        public DateTime DateOfJoin { get; set; }
    }
}
