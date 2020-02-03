using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Models
{
    public class RecruitmentRequestViewModel : ViewModelBase
    {
        public IEnumerable<RecruitmentRequestModel> Requests { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public List<SelectListItem> FilterTypeDropDownItems { get; set; }

        public List<SelectListItem> FilterValueDropDownItems { get; set; }

        public string FilterType { get; set; }

        public string FilterValue { get; set; }

        public RecruitmentRequestViewModel()
        {
            FilterTypeDropDownItems = new List<SelectListItem>();
            FilterValueDropDownItems = new List<SelectListItem>();
            Requests = new List<RecruitmentRequestModel>();
        }

    }
}