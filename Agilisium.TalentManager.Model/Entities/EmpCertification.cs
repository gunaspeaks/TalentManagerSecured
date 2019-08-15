using System;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class EmpCertification : EntityBase
    {
        [Key]
        public int EntryID { get; set; }

        public int EmployeeID { get; set; }

        public int CertificationID { get; set; }

        public DateTime? ValidUpto { get; set; }

        public DateTime? CertifiedOn { get; set; }
    }
}
