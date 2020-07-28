using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class CertificationModel : ViewModelBase
    {
        public bool IsSelected { get; set; }

        public int CertificationID { get; set; }

        [DisplayName("Technology Area")]
        [Required(ErrorMessage = "Please select a Technology Area")]
        public int TechnologyAreaID { get; set; }

        [DisplayName("Technology Area")]
        public string TechnologyArea { get; set; }

        [DisplayName("Certificate Name")]
        [Required(ErrorMessage = "Please enter Certificate Name")]
        public string Name { get; set; }

        [DisplayName("Code")]
        public string ShortName { get; set; }
    }
}