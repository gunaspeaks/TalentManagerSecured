namespace Agilisium.TalentManager.Dto
{
    public class PodWiseHeadCountDto : DtoBase
    {
        public int PracticeID { get; set; }

        public string PracticeName { get; set; }

        public int TotalCount { get; set; }

        public int BillableCount { get; set; }

        public int BenchCount { get; set; }

        public int ComBufferCount { get; set; }

        public int NonComBufferCount { get; set; }

        public int BenchAvailableCount { get; set; }

        public int BenchEarmarkedCount { get; set; }
    }
}
