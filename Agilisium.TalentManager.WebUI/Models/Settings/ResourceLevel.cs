using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class ResourceLevelModel : ViewModelBase
    {
        public int ItemEntryID { get; set; }

        [DisplayName("Parent Level")]
        [Required]
        public int ParentLevelID { get; set; }

        [DisplayName("Parent Level")]
        public string ParentLevel { get; set; }

        [DisplayName("Level Name")]
        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; }
    }
}