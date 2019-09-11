using Agilisium.TalentManager.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpCertificationsViewModel
    {
        public EmpCertificationsViewModel()
        {
            AquiredCertifications = new List<EmpCertificationModel>();
            AvailableCertifications = new List<CertificationModel>();
        }

        public int EmployeeID { get; set; }

        public IEnumerable<EmpCertificationModel> AquiredCertifications { get; set; }

        public List<CertificationModel> AvailableCertifications { get; set; }

    }
}