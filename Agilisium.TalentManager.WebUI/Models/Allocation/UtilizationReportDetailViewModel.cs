using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class UtilizationReportDetailViewModel
    {
        public UtilizationReportDetailViewModel()
        {
            From = DateTime.Today;
            Upto = DateTime.Today;
            Allocations = new List<BillabilityWiseAllocationDetailModel>();

            FilterTypeListItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Employee Name",
                    Value = "emp"
                },
                new SelectListItem
                {
                    Text = "For All Resources",
                    Value = "all"
                },
                new SelectListItem
                {
                    Text = "POD",
                    Value = "pod"
                },
                new SelectListItem
                {
                    Text = "Project Name",
                    Value = "prj"
                },
                new SelectListItem
                {
                    Text = "Account Name",
                    Value = "acc"
                },
                new SelectListItem
                {
                    Text = "Allocation Type",
                    Value = "alt"
                },
            };

            FilterValueListItems = new List<SelectListItem>();
        }

        public List<BillabilityWiseAllocationDetailModel> Allocations { get; set; }

        public List<SelectListItem> FilterTypeListItems { get; set; }

        public List<SelectListItem> FilterValueListItems { get; set; }

        public string FilterType { get; set; }

        public string FilterValue { get; set; }

        [DisplayName("From")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "Please select the From date")]
        public DateTime From { get; set; }

        [DisplayName("Upto")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "Please select the Upto date")]
        public DateTime Upto { get; set; }
    }
}