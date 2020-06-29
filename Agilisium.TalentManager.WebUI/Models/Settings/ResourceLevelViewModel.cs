using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class ResourceLevelViewModel : ViewModelBase
    {
        public IEnumerable<ResourceLevelModel> ResourceLevels { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public ResourceLevelViewModel()
        {
            ResourceLevels = new List<ResourceLevelModel>();
        }
    }
}