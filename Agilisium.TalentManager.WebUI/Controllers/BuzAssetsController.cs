using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Service.Abstract;
using Agilisium.TalentManager.Service.Concreate;
using Agilisium.TalentManager.WebUI.Helpers;
using Agilisium.TalentManager.WebUI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Controllers
{
    public class BuzAssetsController : BaseController
    {
        private readonly IEmployeeTechService techService;

        public BuzAssetsController(IEmployeeTechService techService)
        {
            this.techService = techService;
        }

        // GET: BuzAssets
        [Authorize(Roles = "Human Resource, Super Admin, Admin")]
        public ActionResult List(string findBy = "", int page = 0)
        {
            EmpSkillSummaryViewModel viewModel = new EmpSkillSummaryViewModel()
            {
                ETech = findBy
            };

            try
            {
                InitializeListPage(viewModel);

                int.TryParse(findBy, out int filterValueID);
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = techService.TotalRecordsCount(findBy),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.EmpSkillSummaries = GetTechSummaries(findBy,  page);
                }
                else
                {
                    DisplayWarningMessage("No records to display");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(viewModel);
        }

        private void InitializeListPage(EmpSkillSummaryViewModel viewModel)
        {

        }

        private List<EmpSkillSummaryModel> GetTechSummaries(string findBy, int pageNo)
        {
            List<EmpSkillSummaryDto> summaryDtos = techService.GetAllSkillSummary(findBy, RecordsPerPage, pageNo);
            List<EmpSkillSummaryModel> summaryModels = Mapper.Map<List<EmpSkillSummaryDto>, List<EmpSkillSummaryModel>>(summaryDtos);

            return summaryModels;
        }

    }
}