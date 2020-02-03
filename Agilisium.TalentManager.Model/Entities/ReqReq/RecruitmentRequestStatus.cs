using System;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class RecruitmentRequestStatus : EntityBase
    {
        [Key]
        public int RequestStatusEntryID { get; set; }

        public int RecruitmentRequestID { get; set; }

        public int RequestStatusID { get; set; }

        public DateTime RequestUpdatedOn { get; set; }

        public string Comments { get; set; }

        public int OfferedPositions { get; set; }

        public int JoinedPositions { get; set; }
    }
}
