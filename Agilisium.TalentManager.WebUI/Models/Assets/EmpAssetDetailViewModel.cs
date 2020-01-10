using System.Collections.Generic;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmpAssetDetailViewModel : ViewModelBase
    {
        public List<TechSkillCategoryModel> SkillCategories { get; set; }

        public List<TechSkillModel> AvailableSkills { get; set; }

        public List<EmployeeSkillModel> EmployeeSkills { get; set; }

        public string LogonID { get; set; }

        public int EmployeeEntryID { get; set; }
    }
}