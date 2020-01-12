namespace Agilisium.TalentManager.Dto
{
    public class EmployeeSkillDto : DtoBase
    {
        public int EmployeeSkillID { get; set; }

        public int EmployeeEntryID { get; set; }

        public int TechSkillID { get; set; }

        public string TechSkill { get; set; }

        public int SkillCategoryID { get; set; }

        public string SkillCategory { get; set; }

        public string Rating { get; set; }

        public int RatingID { get; set; }

        public string EmployeeID { get; set; }
    }
}
