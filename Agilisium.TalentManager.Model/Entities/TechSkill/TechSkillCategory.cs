using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class TechSkillCategory : EntityBase
    {
        [Key]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }
    }
}
