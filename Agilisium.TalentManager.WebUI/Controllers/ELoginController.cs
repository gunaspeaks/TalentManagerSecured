using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Service.Abstract;
using Agilisium.TalentManager.WebUI.Helpers;
using Agilisium.TalentManager.WebUI.Models;
using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Controllers
{
    public class ELoginController : BaseController
    {
        private readonly IEmployeeLoginMappingService userService;
        private readonly IEmployeeService empService;
        private readonly ApplicationDbContext context;

        public ELoginController(IEmployeeLoginMappingService userService, IEmployeeService empService)
        {
            this.userService = userService;
            this.empService = empService;
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
            EmployeeLoginMappingModel model = new EmployeeLoginMappingModel();
            try
            {
                GetRolesList();

                if (!id.HasValue)
                {
                    DisplayWarningMessage("Looks like, the employee ID is missing in your request");
                    return View(model);
                }

                if (!userService.Exists(id.Value))
                {
                    DisplayWarningMessage($"We are unable to retrieve the details for the give ID: {id}");
                    return View(model);
                }

                EmployeeLoginMappingDto loginDto = userService.GetByID(id.Value);
                model = Mapper.Map<EmployeeLoginMappingDto, EmployeeLoginMappingModel>(loginDto);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public ActionResult Edit(EmployeeLoginMappingModel model, int? page)
        {
            try
            {
                GetRolesList();
                EmployeeLoginMappingDto loginDto = Mapper.Map<EmployeeLoginMappingModel, EmployeeLoginMappingDto>(model);
                userService.Update(loginDto);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
                return View(model);
            }
            return RedirectToAction("List", new { page });
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult Delete(int? id, int? page)
        {
            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the ID is missing in your request");
                return RedirectToAction("List", new { page });
            }

            try
            {
                userService.Delete(new EmployeeLoginMappingDto { MappingID = id.Value });
                DisplaySuccessMessage("Employee Login details have been deleted successfully");
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
            }
            return RedirectToAction("List", new { page });
        }

        #endregion


        #region Private Methods

        private List<EmployeeLoginMappingModel> GetAllUserMappings(int pageNo)
        {
            List<EmployeeLoginMappingDto> users = userService.GetAll(RecordsPerPage, pageNo);
            List<EmployeeLoginMappingModel> userModels = Mapper.Map<List<EmployeeLoginMappingDto>, List<EmployeeLoginMappingModel>>(users);
            return userModels;
        }

        private void GetEmployeesList()
        {
            List<EmployeeDto> employees = empService.GetAllEmployees("");

            List<SelectListItem> empListItems = (from e in employees
                                                 select new SelectListItem
                                                 {
                                                     Text = $"{e.FirstName} {e.LastName}",
                                                     Value = e.EmployeeEntryID.ToString()
                                                 }).OrderBy(i => i.Text).ToList();

            ViewBag.EmployeeListItems = empListItems;
        }

        private void GetRolesList()
        {
            List<IdentityRole> roles = context.Roles.ToList();

            List<SelectListItem> empListItems = (from e in roles
                                                 select new SelectListItem
                                                 {
                                                     Text = e.Name,
                                                     Value = e.Id
                                                 }).OrderBy(i => i.Text).ToList();

            ViewBag.UserRoleListItems = empListItems;
        }

        #endregion
    }
}