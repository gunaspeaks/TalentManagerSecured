using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class EmployeeLoginMappingViewModel : ViewModelBase
    {
        public IEnumerable<EmployeeLoginMappingModel> Mappings { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public EmployeeLoginMappingViewModel()
        {
            Mappings = new List<EmployeeLoginMappingModel>();
        }
    }
}