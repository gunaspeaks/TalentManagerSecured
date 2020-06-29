using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class BuLevelModel : ViewModelBase
    {
        public int ItemEntryID { get; set; }

        [DisplayName("Business Unit")]
        [Required]
        public int BusinessUnitID { get; set; }

        [DisplayName("Business Unit")]
        public string BusinessUnit { get; set; }

        [DisplayName("Level Name")]
        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }
    }
}