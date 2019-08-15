using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class Certification : EntityBase
    {
        [Key]
        public int CertificationID { get; set; }

        public int TechnologyAreaID { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }
    }
}
