using System;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class Employee : EntityBase
    {
        [Key]
        public int EmployeeEntryID { get; set; }

        public string EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailID { get; set; }

        public int BusinessUnitID { get; set; }

        public DateTime DateOfJoin { get; set; }

        public DateTime? LastWorkingDay { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public int? ReportingManagerID { get; set; }

        public int? UtilizationTypeID { get; set; }

        public int EmploymentTypeID { get; set; }

        public int? VisaCategoryID { get; set; }

        public DateTime? VisaValidUpto { get; set; }

        public int? TechnicalRank { get; set; }

        public string OverallExperience { get; set; }

        public string PassportNo { get; set; }

        public DateTime? PassportValidUpto { get; set; }

        public int? StrengthAreaID { get; set; }

        public bool? IsTechResource { get; set; }

        public int? Level1ID { get; set; }

        public int? Level2ID { get; set; }

        public int? Level3ID { get; set; }

        public int? Level4ID { get; set; }

        public int? Level5ID { get; set; }

        public bool? IsManager { get; set; }
    }
}
