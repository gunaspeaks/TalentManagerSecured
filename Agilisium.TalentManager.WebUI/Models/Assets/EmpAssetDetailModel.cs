using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpAssetDetailModel : ViewModelBase
    {
        public int EmpAssetDetailID { get; set; }

        [DisplayName("Employee ID")]
        [Required]
        public string EmployeeID { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Location")]
        [Required]
        public int LocationID { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Primary Skills")]
        [Required]
        public string PrimarySkills { get; set; }

        [DisplayName("Secondary Skills")]
        public string SecondarySkills { get; set; }

        [DisplayName("Overall Experience")]
        [Required]
        public string OverallExperience { get; set; }

        [DisplayName("Visa Status")]
        [Required]
        public int VisaStatusID { get; set; }

        [DisplayName("Visa Status")]
        public string VisaStatus { get; set; }

        [DisplayName("Designation")]
        public string Designation { get; set; }

        [DisplayName("Logon ID")]
        public string LogonID { get; set; }

        public int EmployeeEntryID { get; set; }
    }

    public class TechSkillCategoryModel
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }
    }

    public class TechSkillModel
    {
        public int TechSkillID { get; set; }

        [DisplayName("Techology")]
        public string TechSkillName { get; set; }

        public int TechSkillCategoryID { get; set; }
    }

}