using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class EmployeeVisaDto
    {
        public int EmployeeEntryID { get; set; }

        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string POD { get; set; }

        public string AllocationType { get; set; }

        public string ProjectName { get; set; }

        public string PrimarySkills { get; set; }

        public string SecondarySkills { get; set; }

        public string VisaCategory { get; set; }

        public string TravelledCountries { get; set; }

        public DateTime? VisaValidity { get; set; }
    }
}
