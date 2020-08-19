using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmployeeModel : ViewModelBase
    {
        public EmployeeModel()
        {
            TravelledCountries = "None";
        }

        public int EmployeeEntryID { get; set; }

        [DisplayName("Employee ID")]
        [Required]
        [MaxLength(10, ErrorMessage = "Employee ID should not exceed 10 characters")]
        public string EmployeeID { get; set; }

        [DisplayName("First Name")]
        [Required]
        [MaxLength(100, ErrorMessage = "First Name should not exceed 100 characters")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [MaxLength(100, ErrorMessage = "Last Name should not exceed 100 characters")]
        public string LastName { get; set; }

        [DisplayName("Email ID")]
        [Required]
        [MaxLength(100, ErrorMessage = "Email ID should not exceed 100 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailID { get; set; }

        [DisplayName("Business Unit")]
        [Required]
        public int BusinessUnitID { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("Date of Join")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        //[DataType(DataType.Date)]
        public DateTime DateOfJoin { get; set; }

        [DisplayName("Last Working Day")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "", ApplyFormatInEditMode =false)]
        //[DataType(DataType.Date)]
        public DateTime? LastWorkingDay { get; set; }

        [DisplayName("Primary Skills")]
        [MaxLength(250, ErrorMessage = "Primary Skills should not exceed 100 characters")]
        [Required]
        public string PrimarySkills { get; set; }

        [DisplayName("Secondary Skills")]
        [MaxLength(100, ErrorMessage = "Secondary Skills should not exceed 100 characters")]
        public string SecondarySkills { get; set; }

        [DisplayName("Reporting Manager")]
        [Required]
        public int? ReportingManagerID { get; set; }

        [DisplayName("Reporting Manager")]
        public string ReportingManagerName { get; set; }

        [DisplayName("Project Manager")]
        public int? ProjectManagerID { get; set; }

        [DisplayName("Project Manager")]
        public string ProjectManagerName { get; set; }

        [DisplayName("Utilization Type")]
        public int? UtilizationTypeID { get; set; }

        [DisplayName("Utilization Type")]
        public string UtilizationTypeName { get; set; }

        [DisplayName("Employment Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select an Employment Type")]
        public int EmploymentTypeID { get; set; }

        [DisplayName("Employment Type")]
        public string EmploymentTypeName { get; set; }

        [DisplayName("Holding Visa?")]
        public int? VisaCategoryID { get; set; }

        [DisplayName("Holding Visa?")]
        public string VisaCategory { get; set; }

        [DisplayName("Visa Validity Upto")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        //[DataType(DataType.Date)]
        public DateTime? VisaValidUpto { get; set; }

        [DisplayName("Technical Rank")]
        [Required]
        public int? TechnicalRank { get; set; }

        [DisplayName("Overall Experience")]
        public string OverallExperience { get; set; }

        [DisplayName("Passport Number")]
        public string PassportNo { get; set; }

        [DisplayName("Passport Validity Upto")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        //[DataType(DataType.Date)]
        public DateTime? PassportValidUpto { get; set; }

        [DisplayName("Travelled Countries")]
        [MaxLength(100, ErrorMessage = "100 Characters maximum")]
        public string TravelledCountries { get; set; }

        [DisplayName("Certifications")]
        public int Certifications { get; set; }

        [DisplayName("Strength Area")]
        public int? StrengthAreaID { get; set; }

        [DisplayName("Strength Area")]
        public string StrengthArea { get; set; }

        [DisplayName("Is Technical?")]
        public bool? IsTechResource { get; set; }

        [DisplayName("Level 1")]
        [Required]
        public int? Level1ID { get; set; }

        [DisplayName("Level 2")]
        [Required]
        public int? Level2ID { get; set; }

        [DisplayName("Level 3")]
        public int? Level3ID { get; set; }

        [DisplayName("Level 4")]
        public int? Level4ID { get; set; }

        [DisplayName("Level 5")]
        public int? Level5ID { get; set; }

        [DisplayName("Level 1")]
        public string Level1 { get; set; }

        [DisplayName("Level 2")]
        public string Level2 { get; set; }

        [DisplayName("Level 3")]
        public string Level3 { get; set; }

        [DisplayName("Level 4")]
        public string Level4 { get; set; }

        [DisplayName("Level 5")]
        public string Level5 { get; set; }

        [DisplayName("Is In Management Role")]
        [Required]
        public bool? IsManager { get; set; }

        [DisplayName("Past Experience")]
        public float? PastExperience { get; set; }

        [DisplayName("Is In Architect Role")]
        public bool? IsArchitect { get; set; }
    }
}