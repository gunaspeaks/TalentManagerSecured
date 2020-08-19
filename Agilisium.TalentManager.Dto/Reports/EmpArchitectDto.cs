using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class EmpArchitectDto
    {
        public string EmployeeName { get; set; }

        public string EmployeeID { get; set; }

        public string AccountName { get; set; }

        public string ProjectName { get; set; }

        public DateTime? AllocatedFrom { get; set; }

        public DateTime? AllocatedUpTo { get; set; }

        public string AllocationType { get; set; }
    }
}
