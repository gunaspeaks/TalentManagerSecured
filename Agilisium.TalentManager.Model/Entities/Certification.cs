using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Model.Entities
{
    public class Certification
    {
        [Key]
        public int CertificationID { get; set; }

        public int TypeID { get; set; }

        public string TechnologyArea { get; set; }

        public string Name { get; set; }

        public DateTime? ValidUpto { get; set; }
    }
}
