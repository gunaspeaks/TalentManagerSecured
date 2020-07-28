using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Models
{

    public class EmployeeSkillsReportViewModel : ViewModelBase
    {

        public EmployeeSkillsReportViewModel()
        {
            EmployeeSkillsReports = new List<EmployeeSkillsReportModel>();
            FilterTypeDropDownItems = new List<SelectListItem>();
            FilterValueDropDownItems = new List<SelectListItem>();
        }

        public IEnumerable<EmployeeSkillsReportModel> EmployeeSkillsReports { get; set; }

        public string FilterBy { get; set; }

        public int? FilterValue { get; set; }

        public string FilterText { get; set; }

        public IEnumerable<SelectListItem> FilterTypeDropDownItems { get; set; }

        public IEnumerable<SelectListItem> FilterValueDropDownItems { get; set; }

        public PagingInfo PagingInfo { get; set; }

    }


    public class EmployeeSkillsReportModel : ViewModelBase
    {
        public int EmployeeEntryID { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Account")]
        public string AccountName { get; set; }

        public int AccountID { get; set; }

        [DisplayName("Project")]
        public string ProjectName { get; set; }

        public int ProjectID { get; set; }

        [DisplayName("Billability")]
        public string AllocationType { get; set; }

        public int AllocationTypeID { get; set; }

        [DisplayName("Overall Experience")]
        public string OverallExperience { get; set; }

        [DisplayName("Limited Skills")]
        public string LimitedSkills { get; set; }

        [DisplayName("Basic Skills")]
        public string BasicSkills { get; set; }

        [DisplayName("Expert Skills")]
        public string ExpertSkills { get; set; }

        [DisplayName("Advanced Skills")]
        public string AdvancedSkills { get; set; }

        [DisplayName("Prof. Skills")]
        public string ProficientSkills { get; set; }

        [DisplayName("Proj. Mngr")]
        public string ProjectManager { get; set; }

        public int ProjectManagerID { get; set; }

        [DisplayName("Reporting Mngr")]
        public string ReportingManager { get; set; }

        public int ReportingManagerID { get; set; }

        [DisplayName("LastWorking Day?")]
        public DateTime? LastWorkingDay { get; set; }

        [DisplayName("Allocated From")]
        public DateTime? AlloctionStartDate { get; set; }

        [DisplayName("Allocated Upto")]
        public DateTime? AllocationEndDate { get; set; }

        [DisplayName("Primary Skills")]
        public string PrimarySkills { get; set; }

        [DisplayName("Secondary Skills")]
        public string SecondarySkills { get; set; }

        [DisplayName("Past Experience")]
        public float? PastExperience { get; set; }

        public DateTime DateOfJoin { get; set; }
    }
}