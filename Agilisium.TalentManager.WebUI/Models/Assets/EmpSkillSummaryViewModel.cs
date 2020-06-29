using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpSkillSummaryViewModel:ViewModelBase
    {
        public EmpSkillSummaryViewModel()
        {
            EmpSkillSummaries = new List<EmpSkillSummaryModel>();
        }

        public List<EmpSkillSummaryModel> EmpSkillSummaries { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string ETech { get; set; }
    }
}