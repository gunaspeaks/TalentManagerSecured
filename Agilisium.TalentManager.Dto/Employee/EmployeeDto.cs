using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class EmployeeDto : DtoBase
    {
        public int EmployeeEntryID { get; set; }

        public string EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailID { get; set; }

        public int BusinessUnitID { get; set; }

        public string BusinessUnitName { get; set; }

        public DateTime DateOfJoin { get; set; }

        public DateTime? LastWorkingDay { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public int? ReportingManagerID { get; set; }

        public string ReportingManagerName { get; set; }

        public string ProjectManagerName { get; set; }

        public int ProjectManagerID { get; set; }

        public int? UtilizationTypeID { get; set; }

        public string UtilizationTypeName { get; set; }

        public int EmploymentTypeID { get; set; }

        public string EmploymentTypeName { get; set; }

        public int? VisaCategoryID { get; set; }

        public string VisaCategory { get; set; }

        public DateTime? VisaValidUpto { get; set; }

        public int? TechnicalRank { get; set; }

        public string OverallExperience { get; set; }

        public string PassportNo { get; set; }

        public DateTime? PassportValidUpto { get; set; }

        public string TravelledCountries { get; set; }

        public int Certifications { get; set; }

        public int? StrengthAreaID { get; set; }

        public string StrengthArea { get; set; }

        public bool? IsTechResource { get; set; }

        public int? Level1ID { get; set; }

        public int? Level2ID { get; set; }

        public int? Level3ID { get; set; }

        public int? Level4ID { get; set; }

        public int? Level5ID { get; set; }

        public string Level1 { get; set; }

        public string Level2 { get; set; }

        public string Level3 { get; set; }

        public string Level4 { get; set; }

        public string Level5 { get; set; }

        public bool? IsManager { get; set; }
    }
}
