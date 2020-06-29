using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class EmpSkillSummaryDto : DtoBase
    {
        public int EmployeeEntryID { get; set; }

        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string LimitedSkills { get; set; }

        public string BasicSkills { get; set; }

        public string ProfSkills { get; set; }

        public string AdvanceSkills { get; set; }

        public string ExpertSkills { get; set; }
    }
}
