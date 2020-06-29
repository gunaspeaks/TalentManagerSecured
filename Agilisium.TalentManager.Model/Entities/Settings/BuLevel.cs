using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class BuLevel : EntityBase
    {
        [Key]
        public int ItemEntryID { get; set; }

        public int BusinessUnitID { get; set; }

        public string ItemName { get; set; }
    }
}
