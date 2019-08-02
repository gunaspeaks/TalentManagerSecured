using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmployeeVisaModel
    {
        public int EmployeeEntryID { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("POD")]
        public string POD { get; set; }

        [DisplayName("Allocation Type")]
        public string AllocationType { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [DisplayName("Primary Skills")]
        public string PrimarySkills { get; set; }

        [DisplayName("Secondary Skills")]
        public string SecondarySkills { get; set; }

        [DisplayName("Visa Category")]
        public string VisaCategory { get; set; }

        [DisplayName("Travelled Countries")]
        public string TravelledCountries { get; set; }

        [DisplayName("Visa Validity")]
        public DateTime? VisaValidity { get; set; }
    }
}