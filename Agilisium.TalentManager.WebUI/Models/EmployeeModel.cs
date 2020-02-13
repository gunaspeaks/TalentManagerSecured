﻿using System;
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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Employee ID is required")]
        [MaxLength(10, ErrorMessage = "Employee ID should not exceed 10 characters")]
        public string EmployeeID { get; set; }

        [DisplayName("First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        [MaxLength(100, ErrorMessage = "First Name should not exceed 100 characters")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        [MaxLength(100, ErrorMessage = "Last Name should not exceed 100 characters")]
        public string LastName { get; set; }

        [DisplayName("Email ID")]
        [MaxLength(100, ErrorMessage = "Email ID should not exceed 100 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailID { get; set; }

        [DisplayName("Business Unit")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a Business Unit")]
        public int BusinessUnitID { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnitName { get; set; }

        [DisplayName("POD")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a Practice")]
        public int PracticeID { get; set; }

        [DisplayName("POD")]
        public string PracticeName { get; set; }

        [DisplayName("Competency")]
        [Required(ErrorMessage = "Please select a Competency")]
        public int SubPracticeID { get; set; }

        [DisplayName("Competency")]
        public string SubPracticeName { get; set; }

        [DisplayName("Date of Join")]
        [Required(ErrorMessage = "Date of Join is required")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        //[DataType(DataType.Date)]
        public DateTime DateOfJoin { get; set; }

        [DisplayName("Last Working Day")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "", ApplyFormatInEditMode =false)]
        //[DataType(DataType.Date)]
        public DateTime? LastWorkingDay { get; set; }

        [DisplayName("Primary Skills")]
        [Required(ErrorMessage = "Primary Skills is required")]
        [MaxLength(100, ErrorMessage = "Primary Skills should not exceed 100 characters")]
        public string PrimarySkills { get; set; }

        [DisplayName("Secondary Skills")]
        [MaxLength(100, ErrorMessage = "Secondary Skills should not exceed 100 characters")]
        public string SecondarySkills { get; set; }

        [DisplayName("Reporting Manager")]
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
        public int? TechnicalRank { get; set; }

        [DisplayName("Total Experience")]
        public float? TotalExperience { get; set; }

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
    }
}