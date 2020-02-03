using System.Collections.Generic;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class UtilizationDashboardViewModel : ViewModelBase
    {
        public UtilizationDashboardViewModel()
        {
            AllocationSummary = new List<BillabilityWiseAllocationSummaryModel>();
            CommittedManagementCount = 0;
            CommittedLabCount = 0;
        }


        public List<BillabilityWiseAllocationSummaryModel> AllocationSummary { get; set; }

        public int CommittedManagementCount { get; set; }

        public int CommittedLabCount { get; set; }
    }
}