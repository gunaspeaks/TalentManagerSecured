using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class ContractorWidgetModel
    {
        [DisplayName("Active Contractors")]
        public int ActiveContractors { get; set; }
    }
}