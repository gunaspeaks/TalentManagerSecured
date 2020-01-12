using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class EmpAssetDetail : EntityBase
    {
        [Key]
        public int EmpAssetDetailID { get; set; }

        public string EmployeeID { get; set; }

        public int LocationID { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public string OverallExperience { get; set; }

        public int VisaStatusID { get; set; }

        public string Designation { get; set; }

        public int EmployeeEntryID { get; set; }
    }
}
