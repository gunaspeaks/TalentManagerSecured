namespace Agilisium.TalentManager.Dto
{
    public class TechSkillCategoryDto : DtoBase
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }
    }

    public class TechSkillDto : DtoBase
    {
        public int TechSkillID { get; set; }

        public string TechSkillName { get; set; }

        public string SkillCategoryName { get; set; }

        public int TechSkillCategoryID { get; set; }
    }
}
