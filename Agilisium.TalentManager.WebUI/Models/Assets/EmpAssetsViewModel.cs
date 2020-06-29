using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpAssetsViewModel : ViewModelBase
    {
        public EmpAssetsViewModel()
        {
            EmployeeSkills = new List<EmployeeSkillModel>();
        }

        public List<EmployeeSkillModel> EmployeeSkills { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string ETech { get; set; }
    }
}