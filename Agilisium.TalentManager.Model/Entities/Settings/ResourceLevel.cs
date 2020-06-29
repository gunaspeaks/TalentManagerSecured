using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class ResourceLevel : EntityBase
    {
        [Key]
        public int ItemEntryID { get; set; }

        public int ParentLevelID { get; set; }

        public string ItemName { get; set; }
    }
}
