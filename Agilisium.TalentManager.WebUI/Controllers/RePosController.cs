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
    public class RePosController : BaseController
    {
        private readonly IEmployeeService empService;
        private readonly IDropDownSubCategoryService subCategoryService;
        private readonly IRecruitmentRequestService reqService;
        private readonly IProjectService projectService;
        private readonly IProjectAccountService accountService;

        public RePosController(IRecruitmentRequestService reqService, IEmployeeService empService,
            IProjectService projectService, IDropDownSubCategoryService subCategoryService, IProjectAccountService accountService)
        {
            this.empService = empService;
            this.subCategoryService = subCategoryService;
            this.reqService = reqService;
            this.projectService = projectService;
            this.accountService = accountService;
        }

        // GET: RePos
        public ActionResult List(string filterType = "", string filterValue = "", int page = 1)
        {
            RecruitmentRequestViewModel viewModel = new RecruitmentRequestViewModel();

            try
            {
                InitializeListPage(viewModel);
                int.TryParse(filterValue, out int filterValueID);
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = reqService.TotalRecordsCount(filterType, filterValueID),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Requests = GetAllRequests(filterType, filterValueID, page);
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(viewModel);
        }

        public ActionResult Create(string filterType, string filterValue, int? page)
        {
            RecruitmentRequestModel requestModel = new RecruitmentRequestModel();

            try
            {
                LoadDropDownItems();
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(requestModel);
        }

        [HttpPost]
        public ActionResult Create(RecruitmentRequestModel request, string filterType, string filterValue, int? page)
        {
            try
            {
                LoadDropDownItems();
                if (ModelState.IsValid)
                {
                    if (reqService.Exists(request.RequestNo))
                    {
                        DisplayWarningMessage("Request No. is duplicate");
                        return View(request);
                    }
                    RecruitmentRequestDto dto = Mapper.Map<RecruitmentRequestModel, RecruitmentRequestDto>(request);
                    reqService.Create(dto);
                    DisplaySuccessMessage($"New Request ({request.RequestNo}) has been created successfully");
                    return RedirectToAction("List", new { filterType, filterValue, page });
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(request);
        }

        public ActionResult Edit(int? id, string filterType, string filterValue, int? page)
        {
            if (id.HasValue == false)
            {
                DisplayWarningMessage("Request ID is missing in your request");
                return RedirectToAction("List", new { filterType, filterValue, page });
            }
            RecruitmentRequestModel requestModel = new RecruitmentRequestModel();

            try
            {
                LoadDropDownItems();
                if (reqService.Exists(id.Value) == false)
                {
                    DisplayWarningMessage("We couldn't find the request details. Please check with the Admin team.");
                }
                RecruitmentRequestDto reqestDto = reqService.GetByID(id.Value);
                requestModel = Mapper.Map<RecruitmentRequestDto, RecruitmentRequestModel>(reqestDto);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(requestModel);
        }

        [HttpPost]
        public ActionResult Edit(RecruitmentRequestModel request, string filterType, string filterValue, int? page)
        {
            try
            {
                LoadDropDownItems();
                if (ModelState.IsValid)
                {
                    if (reqService.Exists(request.RequestNo, request.RecruitmentRequestID))
                    {
                        DisplayWarningMessage("Request No. is duplicate");
                    }
                    else
                    {
                        RecruitmentRequestDto dto = Mapper.Map<RecruitmentRequestModel, RecruitmentRequestDto>(request);
                        reqService.Update(dto);
                        DisplaySuccessMessage($"The Request ({request.RequestNo}) has been updated successfully");
                        return RedirectToAction("List", new { filterType, filterValue, page });
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(request);
        }

        public ActionResult Delete(int? id, string filterType, string filterValue, int? page)
        {
            if (id.HasValue == false)
            {
                DisplayWarningMessage("Request ID is missing in your request");
                return RedirectToAction("List", new { filterType, filterValue, page });
            }
            try
            {
                reqService.Delete(new RecruitmentRequestDto { RecruitmentRequestID = id.Value });
                DisplaySuccessMessage("The request has been deleted successfully");
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
            }
            return RedirectToAction("List", new { filterType, filterValue, page });
        }

        public ActionResult UpdateStatus(int? id, string filterType, string filterValue, int? page)
        {

            if (id.HasValue == false)
            {
                DisplayWarningMessage("Request ID is missing in your request");
                return RedirectToAction("List", new { filterType, filterValue, page });
            }

            RecruitmentRequestStatusModel viewModel = new RecruitmentRequestStatusModel();
            try
            {
                LoadDropDownItemsForStatusPage();
                viewModel = GetRequestStatusEntries(id);
                viewModel.OfferedPosition = 0;
                viewModel.JoinedPosition = 0;

            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(viewModel);
        }

        private RecruitmentRequestStatusModel GetRequestStatusEntries(int? id)
        {
            RecruitmentRequestDto req = reqService.GetByID(id.Value);
            List<RecruitmentRequestStatusDto> statusEntries = reqService.GetStatusEntriesForRequest(id.Value);
            RecruitmentRequestStatusModel viewModel = new RecruitmentRequestStatusModel
            {
                RequestNo = req?.RequestNo,
                RecruitmentRequestID = id.Value,
                OpenPosition = req.TotalPosition - req.OfferedCount - req.JoinedCount,
                OfferedPosition = req.OfferedCount,
                JoinedPosition = req.JoinedCount,
                TotalPosition = req.TotalPosition,
                RequestStatusID = req.OverallStatusID,
            };
            ViewBag.OldStatusEntries = Mapper.Map<List<RecruitmentRequestStatusDto>, List<RecruitmentRequestStatusModel>>(statusEntries);
            return viewModel;
        }

        [HttpPost]
        public ActionResult UpdateStatus(RecruitmentRequestStatusModel request, string filterType, string filterValue, int? page)
        {
            try
            {
                LoadDropDownItemsForStatusPage();
                RecruitmentRequestStatusModel oldReqModel = GetRequestStatusEntries(request.RecruitmentRequestID);

                if (ModelState.IsValid)
                {

                    int calTol = request.OfferedPosition + request.JoinedPosition;
                    if (calTol > oldReqModel.OpenPosition)
                    {
                        DisplayWarningMessage("Cheating! Please check the Total open positions and offered/joined positions");
                        request.TotalPosition = oldReqModel.TotalPosition;
                        request.RequestNo = oldReqModel.RequestNo;
                        request.OpenPosition = oldReqModel.OpenPosition;
                    }
                    else
                    {
                        RecruitmentRequestStatusDto statusEntry = Mapper.Map<RecruitmentRequestStatusModel, RecruitmentRequestStatusDto>(request);
                        reqService.AddRequestStatus(statusEntry);
                        DisplaySuccessMessage($"Status updated successfully");
                        return RedirectToAction("List", new { filterType, filterValue, page });
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(request);
        }

        [HttpPost]
        public JsonResult LoadFilterValueListItems(string filterType)
        {
            List<SelectListItem> filterValues = new List<SelectListItem>();
            switch (filterType.ToLower())
            {
                case "acc":
                    filterValues = GetAccountsList();
                    break;
                case "prj":
                    filterValues = GetProjectsList(-1);
                    break;
                case "wol":
                    filterValues = GetWorkLocationTypes();
                    break;
                case "sts":
                    filterValues = GetRequestStatusList();
                    break;
                case "pri":
                    filterValues = GetRequestPrioritysList();
                    break;
                case "age":
                    filterValues = GetAgingBandList();
                    break;
            }


            filterValues.Insert(0, new SelectListItem
            {
                Text = "Please Select",
                Value = "0",
            });
            Session["FilterValueListItemsRepos"] = filterValues;
            return Json(filterValues);
        }

        [HttpPost]
        public JsonResult LoadProjectsListItems(int accountID)
        {
            List<SelectListItem> filterValues = GetProjectsList(accountID);

            filterValues.Insert(0, new SelectListItem
            {
                Text = "Please Select",
                Value = "0",
            });
            Session["FilterValueListItemsRepos"] = filterValues;
            return Json(filterValues);
        }

        private void InitializeListPage(RecruitmentRequestViewModel viewModel)
        {
            viewModel.FilterTypeDropDownItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Project",
                    Value = "prj"
                },
                new SelectListItem
                {
                    Text = "Account",
                    Value = "acc"
                },
                new SelectListItem
                {
                    Text = "Work Location",
                    Value = "wol"
                },
                new SelectListItem
                {
                    Text = "Request Status",
                    Value = "sts"
                },
                new SelectListItem
                {
                    Text = "Request Priority",
                    Value = "pri"
                },
                    new SelectListItem
                {
                    Text = "Aging Band",
                    Value = "age"
                },
            };

        }

        private void LoadDropDownItems()
        {
            ViewBag.EmployeeListItems = GetEmployeesList();
            ViewBag.ProjectListItems = new List<SelectListItem>();
            ViewBag.BuListItems = GetBuListItems();
            ViewBag.AccountListItems = GetAccountsList();
            ViewBag.WolListItems = GetWorkLocationTypes();
            ViewBag.PriorityListItems = GetRequestPrioritysList();
            ViewBag.AgingBandListItems = GetAgingBandList();
            ViewBag.ReasonListItems = GetReasonTypes();
        }

        private void LoadDropDownItemsForStatusPage()
        {
            ViewBag.StatusListItems = GetRequestStatusList();
        }

        private List<SelectListItem> GetEmployeesList()
        {
            List<EmployeeDto> employees = empService.GetAllEmployees("");

            List<SelectListItem> pmList = (from e in employees
                                           select new SelectListItem
                                           {
                                               Text = $"{e.FirstName} {e.LastName}",
                                               Value = e.EmployeeEntryID.ToString()
                                           }).OrderBy(i => i.Text).ToList();

            return pmList;
        }

        private List<SelectListItem> GetProjectsList(int accountID)
        {
            IEnumerable<ProjectDto> projects = accountID <= 0 ? projectService.GetAll() : projectService.GetAllByAccount(accountID);

            List<SelectListItem> projectList = (from p in projects
                                                orderby p.ProjectName, p.ProjectCode
                                                select new SelectListItem
                                                {
                                                    Text = $"{p.ProjectName}-{p.ProjectCode}",
                                                    Value = p.ProjectID.ToString()
                                                }).OrderBy(i => i.Text).ToList();

            return projectList;
        }

        private List<SelectListItem> GetAccountsList()
        {
            IEnumerable<ProjectAccountDto> accounts = accountService.GetAll();

            List<SelectListItem> accsList = (from a in accounts
                                             orderby a.AccountName
                                             select new SelectListItem
                                             {
                                                 Text = a.AccountName,
                                                 Value = a.AccountID.ToString()
                                             }).OrderBy(i => i.Text).ToList();

            return accsList;
        }

        private List<SelectListItem> GetBuListItems()
        {
            IEnumerable<DropDownSubCategoryDto> reqStatusType = subCategoryService.GetSubCategories((int)CategoryType.BusinessUnit);
            return (from c in reqStatusType
                    orderby c.SubCategoryName
                    select new SelectListItem
                    {
                        Text = c.SubCategoryName,
                        Value = c.SubCategoryID.ToString()
                    }).ToList();
        }

        private List<SelectListItem> GetWorkLocationTypes()
        {
            IEnumerable<DropDownSubCategoryDto> reqStatusType = subCategoryService.GetSubCategories((int)CategoryType.WorkLocationType);
            return (from c in reqStatusType
                    orderby c.SubCategoryName
                    select new SelectListItem
                    {
                        Text = c.SubCategoryName,
                        Value = c.SubCategoryID.ToString()
                    }).ToList();
        }

        private List<SelectListItem> GetReasonTypes()
        {
            IEnumerable<DropDownSubCategoryDto> reqStatusType = subCategoryService.GetSubCategories((int)CategoryType.RequirementReasonType);
            return (from c in reqStatusType
                    orderby c.SubCategoryName
                    select new SelectListItem
                    {
                        Text = c.SubCategoryName,
                        Value = c.SubCategoryID.ToString()
                    }).ToList();
        }

        private List<SelectListItem> GetRequestStatusList()
        {
            IEnumerable<DropDownSubCategoryDto> reqStatusType = subCategoryService.GetSubCategories((int)CategoryType.RequirementRequestStatus);
            return (from c in reqStatusType
                    orderby c.SubCategoryName
                    select new SelectListItem
                    {
                        Text = c.SubCategoryName,
                        Value = c.SubCategoryID.ToString()
                    }).ToList();
        }

        private List<SelectListItem> GetRequestPrioritysList()
        {
            IEnumerable<DropDownSubCategoryDto> reqStatusType = subCategoryService.GetSubCategories((int)CategoryType.RequirementRequestPriority);
            return (from c in reqStatusType
                    orderby c.SubCategoryName
                    select new SelectListItem
                    {
                        Text = c.SubCategoryName,
                        Value = c.SubCategoryID.ToString()
                    }).ToList();
        }

        private List<SelectListItem> GetAgingBandList()
        {
            IEnumerable<DropDownSubCategoryDto> reqStatusType = subCategoryService.GetSubCategories((int)CategoryType.AgingBand);
            return (from c in reqStatusType
                    orderby c.SubCategoryName
                    select new SelectListItem
                    {
                        Text = c.SubCategoryName,
                        Value = c.SubCategoryID.ToString()
                    }).ToList();
        }

        private List<RecruitmentRequestModel> GetAllRequests(string filterType = "", int filterValue = 0, int page = 1)
        {
            List<RecruitmentRequestModel> rePos = new List<RecruitmentRequestModel>();
            List<RecruitmentRequestDto> records = reqService.GetAll(filterType, filterValue, RecordsPerPage, page);
            return Mapper.Map<List<RecruitmentRequestDto>, List<RecruitmentRequestModel>>(records);
        }
    }
}