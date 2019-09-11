using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class ServiceRequestModel : ViewModelBase
    {
        public int ServiceRequestID { get; set; }

        public int VendorID { get; set; }

        [DisplayName("Requested Date")]
        public DateTime RequestedDate { get; set; }

        [DisplayName("Completed Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? CompletedDate { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Requested Skill")]
        public string RequestedSkill { get; set; }

        public int RequestStatusID { get; set; }

        [DisplayName("Request Status")]
        public string RequestStatus { get; set; }

        [DisplayName("Email Message")]
        public string EmailMessage { get; set; }

        public string VendorEmailID { get; set; }

        public bool IsEmailSent { get; set; }
    }
}