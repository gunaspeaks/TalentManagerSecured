using System;
using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class WindowsServiceSettings : EntityBase
    {
        [Key]
        public int ServiceID { get; set; }

        public string ServiceName { get; set; }

        public DateTime? ExecutedDate { get; set; }

        public string ExecutedTime { get; set; }

        public string ExecutionInterval { get; set; }
    }
}
