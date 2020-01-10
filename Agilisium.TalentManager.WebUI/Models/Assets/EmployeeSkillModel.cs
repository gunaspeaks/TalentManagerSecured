using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmployeeSkillModel : ViewModelBase
    {
        public int EntryID { get; set; }

        public int EmployeeSkillID { get; set; }

        [DisplayName("Employee ID")]
        public int EmployeeID { get; set; }

        [DisplayName("Skill Name")]
        public int TechSkillID { get; set; }

        [DisplayName("Skill Name")]
        public string TechSkill { get; set; }

        [DisplayName("Rating")]
        public string Rating { get; set; }

        [DisplayName("Rating")]
        public int RatingID { get; set; }

        [DisplayName("Skill Category")]
        public int SkillCategoryID { get; set; }

        [DisplayName("Skill Category")]
        public string SkillCategory { get; set; }

        public string LogonID { get; set; }
    }
}