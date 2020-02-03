using System;
using System.Collections.Generic;

namespace Agilisium.TalentManager.Dto
{
    public class RecruitmentRequestDto : DtoBase
    {
        public int RecruitmentRequestID { get; set; }

        public string RequestNo { get; set; }

        public int BusinessUnitID { get; set; }

        public string BusinessUnit { get; set; }

        public int AccountID { get; set; }

        public string Account { get; set; }

        public int ProjectID { get; set; }

        public string Project { get; set; }

        public string RequiredSkills { get; set; }

        public bool IsBillable { get; set; }

        public int WorkLocationTypeID { get; set; }

        public string WorkLocationType { get; set; }

        public int RequestReasonID { get; set; }

        public string RequestReason { get; set; }

        public int? ReplacementID { get; set; }

        public string Replacement { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? OfferOrHoldDate { get; set; }

        public int Aging { get; set; }

        public int AgingBandID { get; set; }

        public string AgingBand { get; set; }

        public DateTime ProjectStartDate { get; set; }

        public int OverallStatusID { get; set; }

        public string OverallStatus { get; set; }

        public int JoinedCount { get; set; }

        public int OfferedCount { get; set; }

        public int OpenPosition { get; set; }

        public int TotalPosition { get; set; }

        public int PriorityID { get; set; }

        public string Priority { get; set; }

        public List<RecruitmentRequestStatusDto> RequestStatuseEntries { get;set;}

        public string LattestComment { get; set; }
    }
}
