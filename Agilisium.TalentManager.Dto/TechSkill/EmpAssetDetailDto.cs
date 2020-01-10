using System.Collections.Generic;

namespace Agilisium.TalentManager.Dto
{
    public class EmpAssetDetailDto : DtoBase
    {
        public int EmpAssetDetailID { get; set; }

        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public int LocationID { get; set; }

        public string Location { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public string OverallExperience { get; set; }

        public int VisaStatusID { get; set; }

        public string VisaStatus { get; set; }

        public string Designation { get; set; }

        public string LogonID { get; set; }

        public int EmployeeEntryID { get; set; }
    }
}
