using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class EmployeeSkill : EntityBase
    {
        [Key]
        public int EmployeeSkillID { get; set; }

        public int EmployeeID { get; set; }

        public int RatingID { get; set; }

        public int TechSkillID { get; set; }

        public int SkillCategoryID { get; set; }
    }
}
