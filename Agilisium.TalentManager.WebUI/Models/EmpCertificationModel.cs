using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpCertificationModel
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
        public DateTime? ValidUpto { get; set; }

        [DisplayName("Certified On")]
        public DateTime? CertifiedOn { get; set; }
    }
}