using System;

namespace Agilisium.TalentManager.Dto
{
    public class EmpCertificationDto : DtoBase
    {
        public int EntryID { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public int CertificationID { get; set; }

        public string CertificationName { get; set; }

        public string ShortName { get; set; }

        public DateTime? ValidUpto { get; set; }

        public DateTime? CertifiedOn { get; set; }
    }
}
