using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilisium.TalentManager.Dto
{
    public class CertificationDto : DtoBase
    {
        public bool IsSelected { get; set; }

        public int CertificationID { get; set; }

        public int TechnologyAreaID { get; set; }

        public string TechnologyArea { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }
    }
}
