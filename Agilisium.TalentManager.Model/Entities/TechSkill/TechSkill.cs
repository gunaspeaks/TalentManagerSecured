using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class TechSkill : EntityBase
    {
        [Key]
        public int TechSkillID { get; set; }

        public string TechSkillName { get; set; }

        public int TechSkillCategoryID { get; set; }
    }
}
