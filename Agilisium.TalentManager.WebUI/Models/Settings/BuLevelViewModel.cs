using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class BuLevelViewModel : ViewModelBase
    {
        public IEnumerable<BuLevelModel> BuLevels { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public BuLevelViewModel()
        {
            BuLevels = new List<BuLevelModel>();
        }
    }
}