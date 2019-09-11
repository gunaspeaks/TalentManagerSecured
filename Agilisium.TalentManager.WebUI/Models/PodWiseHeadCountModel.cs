using System.ComponentModel;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class PodWiseHeadCountModel : ViewModelBase
    {
        public int PracticeID { get; set; }

        [DisplayName("POD Name")]
        public string PracticeName { get; set; }

        [DisplayName("Total Count")]
        public int TotalCount { get; set; }

        [DisplayName("Billable")]
        public int BillableCount { get; set; }

        [DisplayName("Bench")]
        public int BenchCount { get; set; }

        [DisplayName("Committed Buffer")]
        public int ComBufferCount { get; set; }

        [DisplayName("Non-Committed Buffer")]
        public int NonComBufferCount { get; set; }

        [DisplayName("Bench Available")]
        public int BenchAvailableCount { get; set; }

        [DisplayName("Bench Earmarked")]
        public int BenchEarmarkedCount { get; set; }
    }
}