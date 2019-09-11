using System;

namespace Agilisium.TalentManager.Dto
{
    public class WindowsServiceSettingsDto : DtoBase
    {
        public int ServiceID { get; set; }

        public string ServiceName { get; set; }

        public DateTime? ExecutedDate { get; set; }

        public string ExecutedTime { get; set; }

        public string ExecutionInterval { get; set; }
    }
}
