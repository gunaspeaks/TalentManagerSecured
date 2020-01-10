namespace Agilisium.TalentManager.Dto
{
    public class EmployeeSkillDto : DtoBase
    {
        public int EntryID { get; set; }

        public int EmployeeSkillID { get; set; }

        public int EmployeeID { get; set; }

        public int TechSkillID { get; set; }

        public string TechSkill { get; set; }

        public int SkillCategoryID { get; set; }

        public string SkillCategory { get; set; }

        public string Rating { get; set; }

        public int RatingID { get; set; }

        public string LogonID { get; set; }
    }
}
