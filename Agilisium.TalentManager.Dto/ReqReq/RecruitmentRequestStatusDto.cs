using System;

namespace Agilisium.TalentManager.Dto
{
    public class RecruitmentRequestStatusDto : DtoBase
    {
        public int RequestStatusEntryID { get; set; }

        public int RecruitmentRequestID { get; set; }

        public int RequestStatusID { get; set; }

        public string RequestStatus { get; set; }

        public DateTime RequestUpdatedOn { get; set; }

        public string Comments { get; set; }

        public string RequestNo { get; set; }

        public int TotalPosition { get; set; }

        public int OpenPosition { get; set; }

        public int OfferedPosition { get; set; }

        public int JoinedPosition { get; set; }
    }
}
