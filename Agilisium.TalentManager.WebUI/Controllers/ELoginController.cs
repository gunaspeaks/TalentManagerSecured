using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Service.Abstract;
using Agilisium.TalentManager.WebUI.Helpers;
using Agilisium.TalentManager.WebUI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Controllers
{
    public class ELoginController : BaseController
    {
        private readonly IEmployeeLoginMappingService userService;
        private readonly ApplicationDbContext context;

        public ELoginController(IEmployeeLoginMappingService userService)
        {
            this.userService = userService;
            context = new ApplicationDbContext();
        }

        #region Public Methods

        // GET: Users
        public ActionResult List(int page = 1)
        {
            EmployeeLoginMappingViewModel viewModel = new EmployeeLoginMappingViewModel();

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = userService.TotalRecordsCount(),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Mappings = GetAllUserMappings(page);
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

        public ActionResult Edit(int? id)
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public ActionResult Edit(EmployeeLoginMappingModel model)
        {
            return View();
        }

        #endregion


        #region Private Methods

        private List<EmployeeLoginMappingModel> GetAllUserMappings(int pageNo)
        {
            List<EmployeeLoginMappingDto> users = userService.GetAll(RecordsPerPage, pageNo);
            List<EmployeeLoginMappingModel> userModels = Mapper.Map<List<EmployeeLoginMappingDto>, List<EmployeeLoginMappingModel>>(users);
            return userModels;
        }

        #endregion
    }
}