using System;
using System.Collections.Generic;

namespace Agilisium.TalentManager.Model.Entities
{
    public class RecruitmentRequest : EntityBase
    {
        public int RecruitmentRequestID { get; set; }

        public string RequestNo { get; set; }

        public int BusinessUnitID { get; set; }

        public int AccountID { get; set; }

        public int ProjectID { get; set; }

        public string RequiredSkills { get; set; }

        public bool IsBillable { get; set; }

        public int WorkLocationTypeID { get; set; }

        public int RequestReasonID { get; set; }

        public int? ReplacementID { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? OfferOrHoldDate { get; set; }

        public int AgingBandID { get; set; }

        public int JoinedPosition { get; set; }

        public int OfferedPosition { get; set; }

        public int TotalPosition { get; set; }

        public int PriorityID { get; set; }

        public int OverallStatusID { get; set; }

        public List<RecruitmentRequestStatus> RequestStatuses { get; set; }
    }
}
