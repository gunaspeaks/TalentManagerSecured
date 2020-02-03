using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class RecruitmentRequestModel : ViewModelBase
    {
        public RecruitmentRequestModel()
        {
            RequestedDate = DateTime.Now;
            TotalPosition = 1;
        }

        public int RecruitmentRequestID { get; set; }

        [Display(Name = "Request No")]
        [Required]
        public string RequestNo { get; set; }

        [Display(Name = "Business Unit")]
        [Required]
        public int BusinessUnitID { get; set; }

        [Display(Name = "Business Unit")]
        public string BusinessUnit { get; set; }

        [Display(Name = "Account")]
        [Required]
        public int AccountID { get; set; }

        [Display(Name = "Account")]
        public string Account { get; set; }

        [Display(Name = "Project")]
        [Required]
        public int ProjectID { get; set; }

        [Display(Name = "Project")]
        public string Project { get; set; }

        [Display(Name = "Skills Required")]
        [Required]
        public string RequiredSkills { get; set; }

        [Display(Name = "Billable?")]
        public bool IsBillable { get; set; }

        [Display(Name = "Work Location")]
        [Required]
        public int WorkLocationTypeID { get; set; }

        [Display(Name = "Work Location")]
        public string WorkLocationType { get; set; }

        [Display(Name = "Reason for Recruitment")]
        [Required]
        public int RequestReasonID { get; set; }

        [Display(Name = "Fulfilment Type")]
        public string RequestReason { get; set; }

        [Display(Name = "Replacement For")]
        public int? ReplacementID { get; set; }

        [Display(Name = "Replacement For")]
        public string Replacement { get; set; }

        [Display(Name = "Requested On")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime RequestedDate { get; set; }

        [Display(Name = "Offer/Hold Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? OfferOrHoldDate { get; set; }

        [Display(Name = "Aging")]
        public int Aging { get; set; }

        [Display(Name = "Aging Band")]
        [Required]
        public int AgingBandID { get; set; }

        [Display(Name = "Aging Band")]
        public string AgingBand { get; set; }

        [Display(Name = "Project Start Date")]
        public DateTime ProjectStartDate { get; set; }

        [Display(Name = "Joined")]
        public int JoinedCount { get; set; }

        [Display(Name = "Offered")]
        public int OfferedCount { get; set; }

        [Display(Name = "Open")]
        public int OpenPosition { get; set; }

        [Display(Name = "Total Position")]
        [Required]
        public int TotalPosition { get; set; }

        [Display(Name = "Priority")]
        [Required]
        public int PriorityID { get; set; }

        [Display(Name = "Priority")]
        public string Priority { get; set; }

        [Display(Name = "Status")]
        public int OverallStatusID { get; set; }

        [Display(Name = "Status")]
        public string OverallStatus { get; set; }

        [Display(Name = "Comment")]
        public string LattestComment { get; set; }

        public List<RecruitmentRequestStatusModel> RequestStatuseEntries { get; set; }
    }
}