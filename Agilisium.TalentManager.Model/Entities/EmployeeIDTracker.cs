﻿using System.ComponentModel.DataAnnotations;

namespace Agilisium.TalentManager.Model.Entities
{
    public class EmployeeIDTracker
    {
        [Key]
        public int TrackerID { get; set; }

        public int EmploymentTypeID { get; set; }

        public string IDPrefix { get; set; }

        public int RunningID { get; set; }
    }
}
