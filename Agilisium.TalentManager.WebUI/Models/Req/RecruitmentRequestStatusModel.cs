using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class RecruitmentRequestStatusModel : ViewModelBase
    {
        public RecruitmentRequestStatusModel()
        {
            RequestUpdatedOn = DateTime.Now;
        }

        public int RecruitmentRequestID { get; set; }

        public int RequestStatusEntryID { get; set; }

        [Display(Name = "Request Status")]
        public int RequestStatusID { get; set; }

        [Display(Name = "Request No")]
        public string RequestNo { get; set; }

        [Display(Name = "Request Status")]
        public string RequestStatus { get; set; }

        [Display(Name = "Request Updated On")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime RequestUpdatedOn { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Open Position(s)")]
        public int OpenPosition { get; set; }

        [Display(Name = "Offered Position(s)")]
        public int OfferedPosition { get; set; }

        [Display(Name = "Joined Position(s)")]
        public int JoinedPosition { get; set; }

        [Display(Name = "Total Position(s)")]
        public int TotalPosition { get; set; }
    }

    public class RequestStatusViewModel : ViewModelBase
    {
        public RequestStatusViewModel()
        {
            NewStatusEntry = new RecruitmentRequestStatusModel();
            OldStatusEntries = new List<RecruitmentRequestStatusModel>();
        }

        public RecruitmentRequestStatusModel NewStatusEntry { get; set; }

        public List<RecruitmentRequestStatusModel> OldStatusEntries { get; set; }
    }
}