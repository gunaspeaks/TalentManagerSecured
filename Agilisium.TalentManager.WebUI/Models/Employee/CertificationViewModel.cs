using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class CertificationViewModel : ViewModelBase
    {
        public CertificationViewModel()
        {
            Certifications = new List<CertificationModel>();
            TAListItems = new List<SelectListItem>();
        }

        public IEnumerable<CertificationModel> Certifications { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public int SelectedCertID { get; set; }

        public IEnumerable<SelectListItem> TAListItems { get; set; }
    }
}