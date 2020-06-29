using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Service.Abstract;
using Agilisium.TalentManager.WebUI.Helpers;
using Agilisium.TalentManager.WebUI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IEmployeeService empService;
        private readonly IDropDownSubCategoryService subCategoryService;
        private readonly IPracticeService practiceService;
        private readonly ISubPracticeService subPracticeService;
        private readonly IProjectService projectService;
        private readonly IProjectAccountService accountsService;

        public ProjectController(IEmployeeService empService,
            IDropDownSubCategoryService subCategoryService,
            IPracticeService practiceService,
            ISubPracticeService subPracticeService, IProjectService projectService,
            IProjectAccountService accountsService)
        {
            this.empService = empService;
            this.subCategoryService = subCategoryService;
            this.practiceService = practiceService;
            this.subPracticeService = subPracticeService;
            this.projectService = projectService;
            this.accountsService = accountsService;
        }

        // GET: Project
        public ActionResult List(string filterType, string filterValue, int page = 1)
        {
            ProjectViewModel viewModel = new ProjectViewModel()
            {
                FilterType = filterType,
                FilterValue = filterValue
            };

            try
            {
                InitializeListPage(viewModel);

                int.TryParse(filterValue, out int filterValueID);
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = projectService.TotalRecordsCount(filterType, filterValueID),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Projects = GetProjects(filterType, filterValueID, page);
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

        // GET: Project/Create
        public ActionResult Create()
        {
            ProjectModel project = new ProjectModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(15)
            };

            try
            {
                InitializePageData();
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(project);
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(ProjectModel project, string filterType, string filterValue, int? page)
        {
            try
            {
                InitializePageData();

                if (ModelState.IsValid)
                {
                    if (project.EndDate <= project.StartDate)
                    {
                        DisplayWarningMessage("The End date should be greater than the Start date");
                        return View(project);
                    }

                    if (projectService.Exists(project.ProjectName))
                    {
                        DisplayWarningMessage($"The Project Name '{project.ProjectName}' is duplicate");
                        return View(project);
                    }

                    if (projectService.IsDuplicateProjectCode(project.ProjectCode))
                    {
                        DisplayWarningMessage("The Project Code looks duplicated");
                        return View(project);
                    }

                    ProjectDto projectDto = Mapper.Map<ProjectModel, ProjectDto>(project);
                    projectService.Create(projectDto);
                    DisplaySuccessMessage("New Project details have been stored successfully");
                    return RedirectToAction("List", new { filterType, filterValue, page });
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(project);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            ProjectModel projectModel = new ProjectModel();

            try
            {
                InitializePageData();
                if (!id.HasValue)
                {
                    DisplayWarningMessage("Looks like, the ID is missing in your request");
                    return RedirectToAction("List");
                }

                if (!projectService.Exists(id.Value))
                {
                    DisplayWarningMessage("Sorry, we couldn't find the Project details");
                    return RedirectToAction("List");
                }

                ProjectDto emp = projectService.GetByID(id.Value);
                projectModel = Mapper.Map<ProjectDto, ProjectModel>(emp);
                return View(projectModel);
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }

            return View(projectModel);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(ProjectModel project, string filterType, string filterValue, int? page)
        {
            try
            {
                InitializePageData();
                if (ModelState.IsValid)
                {
                    if (project.EndDate <= project.StartDate)
                    {
                        DisplayWarningMessage("The End date should be greater than the Start date");
                        return View(project);
                    }

                    if (projectService.Exists(project.ProjectName, project.ProjectID))
                    {
                        DisplayWarningMessage($"The Project Name '{project.ProjectName}' is duplicate");
                        return View(project);
                    }

                    if (projectService.IsDuplicateProjectCode(project.ProjectCode, project.ProjectID))
                    {
                        DisplayWarningMessage("The Project Code looks duplicated");
                        return View(project);
                    }

                    ProjectDto projectDto = Mapper.Map<ProjectModel, ProjectDto>(project);
                    projectService.Update(projectDto);
                    DisplaySuccessMessage("Project details have been updated successfully");
                    return RedirectToAction("List", new { filterType, filterValue, page });
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(project);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id, string filterType, string filterValue, int? page)
        {
            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the Project ID is missing in your request");
                return RedirectToAction("List", new { filterType, filterValue, page });
            }

            try
            {
                if (projectService.IsReservedEntry(id.Value))
                {
                    DisplayWarningMessage("Hey, why do you want to delete a system or reserved project. Please check with the system administrator for your needs.");
                    return RedirectToAction("List", new { filterType, filterValue, page });
                }
                projectService.Delete(new ProjectDto { ProjectID = id.Value });
                DisplaySuccessMessage("Project details have been deleted successfully");
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
            }

            return RedirectToAction("List", new { filterType, filterValue, page });
        }

        [HttpPost]
        public JsonResult SubPracticeList(int id)
        {
            IEnumerable<SubPracticeDto> subPracticeList = subPracticeService.GetAllByPracticeID(id);
            List<SelectListItem> ddList = (from c in subPracticeList
                                           orderby c.SubPracticeName
                                           select new SelectListItem
                                           {
                                               Text = c.SubPracticeName,
                                               Value = c.SubPracticeID.ToString()
                                           }).ToList();
            ddList.Insert(0, new SelectListItem
            {
                Text = "Please Select",
                Value = "0",
            });
            return Json(ddList);
        }

        [HttpPost]
        public JsonResult GetProjectManagerName(int projectID)
        {
            ProjectDto project = projectService.GetByID(projectID);
            int managerID = project.ProjectManagerID;
            EmployeeDto emp = empService.GetEmployee(managerID);
            string managerName = "Not found";
            if (emp != null)
            {
                managerName = $"{emp.LastName}, {emp.FirstName}";
            }
            JsonResult res = Json(managerName);
            return res;
        }

        [HttpPost]
        public JsonResult GenerateProjectCode(int accountID)
        {
            string projectCode = projectService.GenerateProjectCode(accountID);
            JsonResult res = Json(projectCode);
            return res;
        }

        [HttpPost]
        public JsonResult LoadFilterValueListItems(string filterType)
        {
            List<SelectListItem> filterValues = new List<SelectListItem>();
            switch (filterType)
            {
                case "Business Unit":
                    filterValues = GetBuList();
                    break;
                case "Account":
                    filterValues = GetAllAccountsList();
                    break;
                case "Project Type":
                    filterValues = GetProjectTypeList();
                    break;
            }


            filterValues.Insert(0, new SelectListItem
            {
                Text = "Please Select",
                Value = "0",
            });
            Session["FilterValueListItems"] = filterValues;
            return Json(filterValues);
        }

        private IEnumerable<ProjectModel> GetProjects(string filterType, int filterValue, int pageNo)
        {
            IEnumerable<ProjectDto> projects = projectService.GetAll(filterType, filterValue, RecordsPerPage, pageNo);
            IEnumerable<ProjectModel> projectModels = Mapper.Map<IEnumerable<ProjectDto>, IEnumerable<ProjectModel>>(projects);

            return projectModels;
        }

        private void InitializePageData()
        {
            ViewData["IsNewProject"] = true;
            ViewBag.BuListItems = GetBuList();
            ViewBag.ProjectTypeListItems = GetProjectTypeList();

            GetAllManagersList();
            ViewBag.AccountsListItems = GetAllAccountsList();
            ViewBag.SubPracticeListItems = new List<SelectListItem>
            {
                new SelectListItem{Text="Please Select", Value="0"}
            };
        }

        private void InitializeListPage(ProjectViewModel viewModel)
        {
            viewModel.FilterTypeDropDownItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Account",
                    Value = "Account"
                },
                new SelectListItem
                {
                    Text = "Business Unit",
                    Value = "Business Unit"
                },
                new SelectListItem
                {
                    Text = "Project Type",
                    Value = "Project Type"
                }
            };

            if (string.IsNullOrEmpty(viewModel.FilterType) == false && Session["FilterValueListItems"] != null)
            {
                viewModel.FilterValueDropDownItems = (List<SelectListItem>)Session["FilterValueListItems"];
            }
        }

        private List<SelectListItem> GetProjectTypeList()
        {
            IEnumerable<DropDownSubCategoryDto> buList = subCategoryService.GetAll();
            List<SelectListItem> projectTypeItems = (from c in buList
                                                     orderby c.SubCategoryName
                                                     where c.CategoryID == (int)CategoryType.ProjectType
                                                     select new SelectListItem
                                                     {
                                                         Text = c.SubCategoryName,
                                                         Value = c.SubCategoryID.ToString()
                                                     }).ToList();

            return projectTypeItems;
        }

        private List<SelectListItem> GetBuList()
        {
            IEnumerable<DropDownSubCategoryDto> buList = subCategoryService.GetAll();
            List<SelectListItem> buListItems = (from c in buList
                                                orderby c.SubCategoryName
                                                where c.CategoryID == (int)CategoryType.BusinessUnit
                                                select new SelectListItem
                                                {
                                                    Text = c.SubCategoryName,
                                                    Value = c.SubCategoryID.ToString()
                                                }).ToList();

            return buListItems;
        }
        public JsonResult GetProjectsListItems(int accountID)
        {
            List<SelectListItem> filterValues = GetAllProjectsList(accountID);

            filterValues.Insert(0, new SelectListItem
            {
                Text = "Please Select",
                Value = "0",
            });
            Session["FilterValueListItems"] = filterValues;
            return Json(filterValues);
        }

        private List<SelectListItem> GetAllProjectsList(int accountID)
        {
            List<ProjectDto> projects = projectService.GetAllByAccount(accountID);

            List<SelectListItem> accDDList = (from e in projects
                                              orderby e.ProjectName
                                              select new SelectListItem
                                              {
                                                  Text = e.ProjectName + " (" + e.ProjectCode + ")",
                                                  Value = e.ProjectID.ToString()
                                              }).ToList();

            return accDDList;
        }

        private void GetAllManagersList()
        {
            List<EmployeeDto> employees = empService.GetAllManagers();

            List<SelectListItem> empDDList = (from e in employees
                                              select new SelectListItem
                                              {
                                                  Text = $"{e.FirstName} {e.LastName}",
                                                  Value = e.EmployeeEntryID.ToString()
                                              }).OrderBy(i => i.Text).ToList();

            ViewBag.ProjectManagerListItems = empDDList;
        }

        private List<SelectListItem> GetAllAccountsList()
        {
            List<ProjectAccountDto> accounts = accountsService.GetAll();

            List<SelectListItem> accDDList = (from e in accounts
                                              select new SelectListItem
                                              {
                                                  Text = e.AccountName,
                                                  Value = e.AccountID.ToString()
                                              }).ToList();

            return accDDList;
        }
    }
}
