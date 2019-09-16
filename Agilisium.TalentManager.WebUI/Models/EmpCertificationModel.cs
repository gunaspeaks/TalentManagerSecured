using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpCertificationModel : ViewModelBase
    {
        public int EntryID { get; set; }

        [DisplayName("Employee Name")]
        public int EmployeeID { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Certification")]
        public int CertificationID { get; set; }

        [DisplayName("Certificate Name")]
        public string CertificationName { get; set; }

        [DisplayName("Code")]
        public string ShortName { get; set; }

        [DisplayName("Valid Upto")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ValidUpto { get; set; }

        [DisplayName("Certified On")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CertifiedOn { get; set; }
    }
}