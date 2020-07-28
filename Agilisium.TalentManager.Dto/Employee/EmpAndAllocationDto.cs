using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class EmpAndAllocationDto
    {
        public int EmployeeEntryID { get; set; }

        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public string ReportingManager { get; set; }

        public string EmploymentType { get; set; }

        public string VisaCategory { get; set; }

        public DateTime? VisaValidUpto { get; set; }

        public int? TechnicalRank { get; set; }

        public float? PastExperience { get; set; }

        public string AllocationType { get; set; }

        public string ProjectManager { get; set; }

        public DateTime? AllocationStartDate { get; set; }

        public DateTime? AllocationEndDate { get; set; }

        public string AccountName { get; set; }

        public string ProjectName { get; set; }

        public string ProjectType { get; set; }

        public string BusinessUnit { get; set; }

        public string StrengthArea { get; set; }

        public DateTime DateOfJoin { get; set; }
    }
}
