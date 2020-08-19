using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Service.Abstract;
using Agilisium.TalentManager.WebUI.Helpers;
using Agilisium.TalentManager.WebUI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IAllocationService allocationService;
        private readonly IProjectService projectService;
        private readonly IEmployeeService empService;
        private readonly IPracticeService practiceService;
        private readonly IProjectAccountService accountsService;
        private readonly IDropDownSubCategoryService subCategoryService;
        private readonly IEmployeeTechService techService;

        public ReportsController(IAllocationService allocationService, IProjectService projectService,
            IEmployeeService empService, IPracticeService practiceService, IProjectAccountService accountsService,
            IDropDownSubCategoryService subCategoryService, IEmployeeTechService techService)
        {
            this.allocationService = allocationService;
            this.projectService = projectService;
            this.empService = empService;
            this.practiceService = practiceService;
            this.accountsService = accountsService;
            this.subCategoryService = subCategoryService;
            this.techService = techService;
        }

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManagerWiseAllocations()
        {
            List<ManagerWiseAllocationModel> managerSummary = new List<ManagerWiseAllocationModel>();
            try
            {
                List<ManagerWiseAllocationDto> summaryDtos = allocationService.GetManagerWiseAllocationSummary();
                managerSummary = Mapper.Map<List<ManagerWiseAllocationDto>, List<ManagerWiseAllocationModel>>(summaryDtos);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(managerSummary);
        }

        public ActionResult ManagerWiseProjects(int id)
        {
            ProjectViewModel model = new ProjectViewModel();
            try
            {
                List<ProjectDto> summaryDtos = projectService.GetAllByManagerID(id);
                model.Projects = Mapper.Map<List<ProjectDto>, List<ProjectModel>>(summaryDtos);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult UtilizationReportSummary()
        {
            List<BillabilityWiseAllocationSummaryModel> allocations = new List<BillabilityWiseAllocationSummaryModel>();
            try
            {
                List<BillabilityWiseAllocationSummaryDto> summaryDtos = allocationService.GetBillabilityWiseAllocationSummary();
                allocations = Mapper.Map<List<BillabilityWiseAllocationSummaryDto>, List<BillabilityWiseAllocationSummaryModel>>(summaryDtos);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(allocations);
        }

        public ActionResult UtilizationReportDetail(string filterType, string filterValue)
        {
            UtilizationReportDetailViewModel model = new UtilizationReportDetailViewModel();
            try
            {
                model.FilterType = filterType;
                model.FilterValue = filterValue;
                List<BillabilityWiseAllocationDetailDto> summaryDtos = allocationService.GetBillabilityWiseAllocationDetail(filterType, filterValue);
                model.Allocations = Mapper.Map<List<BillabilityWiseAllocationDetailDto>, List<BillabilityWiseAllocationDetailModel>>(summaryDtos);
                model.FilterValueListItems = GetFilterValueListItems(filterType);
                //LoadFilterValueListItems(filterType);
                if (model.Allocations.Count() == 0)
                {
                    DisplayWarningMessage("No records found. Please try with different filter conditions");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(model);
        }

        public ActionResult UtilizedDaysSummary(string filterType, string filterValue, string sortBy = "ename", string sortType = "asc")
        {
            UtilizedDaysViewModel model = new UtilizedDaysViewModel()
            {
                FilterType = filterType,
                FilterValue = filterValue,
                SortBy = sortBy,
                SortType = sortType
            };

            try
            {
                model.FilterType = filterType;
                model.FilterValue = filterValue;
                List<UtilizedDaysSummaryDto> summaryDtos = allocationService.GetUtilizedDaysSummary(filterType, filterValue, sortBy, sortType);
                model.Employees = Mapper.Map<List<UtilizedDaysSummaryDto>, List<UtilizedDaysSummaryModel>>(summaryDtos);
                if (model.Employees.Count() == 0)
                {
                    DisplayWarningMessage("No records found");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(model);
        }

        public ActionResult GetArchsList()
        {
            List<ArchitectEmpModel> model = new List<ArchitectEmpModel>();
            try
            {
                List<EmpArchitectDto> summaryDtos = allocationService.GetAllArchitectEmployees();
                model = Mapper.Map<List<EmpArchitectDto>, List<ArchitectEmpModel>>(summaryDtos);
                if (model.Count() == 0)
                {
                    DisplayWarningMessage("No records found.");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult LoadFilterValueListItems(string filterType)
        {
            List<SelectListItem> filterValues = null;
            try
            {
                filterValues = GetFilterValueListItems(filterType);

                filterValues.Insert(0, new SelectListItem
                {
                    Text = "Please Select",
                    Value = "0",
                });
                Session["FilterValueListItems"] = filterValues;
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return Json(filterValues);
        }

        public FileStreamResult DownloadAllocationDetails(string filterType, string filterValue)
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Primary Skills,Secondary Skills,Business Unit,Project Name,Account Name,Allocation Type,Allocation Start Date,Allocation End Date,Project Manager,Comments{Environment.NewLine}");
            try
            {
                List<BillabilityWiseAllocationDetailDto> detailsDtos = allocationService.GetBillabilityWiseAllocationDetail(filterType, filterValue);
                foreach (BillabilityWiseAllocationDetailDto dto in detailsDtos)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.PrimarySkills?.Replace(",", ";")},");
                    recordString.Append($"{dto.SecondarySkills?.Replace(",", ";")},");
                    recordString.Append($"{dto.BusinessUnit},");
                    recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.AccountName},");
                    recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.AllocationStartDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.AllocationEndDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.ProjectManager},");
                    recordString.Append($"{dto.Comments}{Environment.NewLine}");
                }

            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", "Resource Allocation Report.csv");
        }

        public FileStreamResult DownloadArchitectEmps()
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Account Name,Project Name,Allocation Type,Allocation From,Allocation Upto{Environment.NewLine}");
            try
            {
                List<EmpArchitectDto> detailsDtos = allocationService.GetAllArchitectEmployees();
                foreach (EmpArchitectDto dto in detailsDtos)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.AccountName},");
                    recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.AllocatedFrom?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.AllocatedUpTo?.ToString("dd/MMM/yyyy")}{Environment.NewLine}");
                }

            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
                return File(stream, "application/vnd.ms-excel", "Agilisium-Architects.csv");
        }

        public FileStreamResult DownloadUtilizedDaysSummary(string filterType, string filterValue)
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Date of Join,Last Allocation End Date,Last Allocation Age In Days,Number Of Allocations{Environment.NewLine}");
            try
            {
                List<UtilizedDaysSummaryDto> summaryDtos = allocationService.GetUtilizedDaysSummary(filterType, filterValue);
                foreach (UtilizedDaysSummaryDto dto in summaryDtos)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.DateOfJoin.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.LastAllocatedDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.AgingDays},");
                    recordString.Append($"{dto.AnyAllocation}{Environment.NewLine}");
                }

            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", "Resource Allocation Report.csv");
        }

        [HttpGet]
        public ActionResult VisaHoldingEmployees()
        {
            List<EmployeeVisaModel> entries = new List<EmployeeVisaModel>();
            try
            {
                List<EmployeeVisaDto> summaryDtos = empService.GetVisaHolderingEmployees();
                entries = Mapper.Map<List<EmployeeVisaDto>, List<EmployeeVisaModel>>(summaryDtos);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(entries);
        }

        public FileStreamResult DownloadVisaHoldingEmployees(string filterType, string filterValue)
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Allocation Type,Project Name,Primary Skills,Secondary Skills,Visa Category,Visa Valid Upto,Travelled Countries{Environment.NewLine}");
            try
            {
                List<EmployeeVisaDto> summaryDtos = empService.GetVisaHolderingEmployees();
                foreach (EmployeeVisaDto dto in summaryDtos)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.PrimarySkills},");
                    recordString.Append($"{dto.SecondarySkills},");
                    recordString.Append($"{dto.VisaCategory},");
                    recordString.Append($"{dto.VisaValidity?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.TravelledCountries}{Environment.NewLine}");
                }

            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", "Resource Allocation Report.csv");
        }

        [HttpGet]
        public ActionResult PodWiseCount()
        {
            List<PodWiseHeadCountModel> entries = new List<PodWiseHeadCountModel>();
            try
            {
                List<PodWiseHeadCountDto> summaryDtos = allocationService.GetPodWiseAllocationCount();
                entries = Mapper.Map<List<PodWiseHeadCountDto>, List<PodWiseHeadCountModel>>(summaryDtos);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(entries);
        }

        public FileStreamResult DownloadPodWiseCount(string filterType, string filterValue)
        {
            StringBuilder recordString = new StringBuilder($"POD Name,Total Count,Billable,Committed Buffer,Non-Committed Buffer,Bench Available,Bench Earmarked{Environment.NewLine}");
            try
            {
                List<PodWiseHeadCountDto> summaryDtos = allocationService.GetPodWiseAllocationCount();
                foreach (PodWiseHeadCountDto dto in summaryDtos)
                {
                    recordString.Append($"{dto.PracticeName},");
                    recordString.Append($"{dto.TotalCount},");
                    recordString.Append($"{dto.BillableCount},");
                    recordString.Append($"{dto.ComBufferCount},");
                    recordString.Append($"{dto.NonComBufferCount},");
                    recordString.Append($"{dto.BenchAvailableCount},");
                    recordString.Append($"{dto.BenchEarmarkedCount}{Environment.NewLine}");
                }

                recordString.Append("Total Count,");
                recordString.Append($"{summaryDtos.Sum(p => p.TotalCount)},");
                recordString.Append($"{summaryDtos.Sum(p => p.BillableCount)},");
                recordString.Append($"{summaryDtos.Sum(p => p.ComBufferCount)},");
                recordString.Append($"{summaryDtos.Sum(p => p.NonComBufferCount)},");
                recordString.Append($"{summaryDtos.Sum(p => p.BenchAvailableCount)},");
                recordString.Append($"{summaryDtos.Sum(p => p.BenchEarmarkedCount)},");
                recordString.Append($"{summaryDtos.Sum(p => p.BenchCount)}{Environment.NewLine}");


            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", "Pod-Wise Resource Count.csv");
        }

        public ActionResult BillableAllocations(DateTime? from, DateTime? upto)
        {
            UtilizationReportDetailViewModel model = new UtilizationReportDetailViewModel();
            try
            {
                if (from.HasValue == false || upto.HasValue == false)
                {
                    DisplayWarningMessage("No records found. Please try with different dates");
                }
                else
                {
                    List<BillabilityWiseAllocationDetailDto> summaryDtos = allocationService.GetAllocationsForDates(from.Value, upto.Value);
                    model.From = from.Value;
                    model.Upto = upto.Value;
                    model.Allocations = Mapper.Map<List<BillabilityWiseAllocationDetailDto>, List<BillabilityWiseAllocationDetailModel>>(summaryDtos);
                    if (model.Allocations.Count() == 0)
                    {
                        DisplayWarningMessage("No records found. Please try with different dates");
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(model);
        }

        public FileStreamResult DownloadBillableAllocations(DateTime? from, DateTime? upto)
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Primary Skills,Secondary Skills,Business Unit,Project Name,Account Name,Allocation Type,Allocation Start Date,Allocation End Date,Project Manager,Comments{Environment.NewLine}");
            try
            {
                List<BillabilityWiseAllocationDetailDto> detailsDtos = allocationService.GetAllocationsForDates(from.Value, upto.Value);
                foreach (BillabilityWiseAllocationDetailDto dto in detailsDtos)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.PrimarySkills?.Replace(",", ";")},");
                    recordString.Append($"{dto.SecondarySkills?.Replace(",", ";")},");
                    recordString.Append($"{dto.BusinessUnit},");
                    recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.AccountName},");
                    recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.AllocationStartDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.AllocationEndDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.ProjectManager},");
                    recordString.Append($"{dto.Comments}{Environment.NewLine}");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", "Billable Allocations Report.csv");
        }

        public ActionResult GetEmpSkillsReport(string filterBy, int? filterValue, string filterText = "", int page = 1)
        {
            EmployeeSkillsReportViewModel viewModel = new EmployeeSkillsReportViewModel()
            {
                FilterBy = filterBy,
                FilterValue = filterValue,
                FilterText = filterText,
            };

            try
            {
                InitializeEmpSkillsReportPage(viewModel);

                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = 0,
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                viewModel.PagingInfo.TotalRecordsCount = techService.GetEmployeeSkillsReportCount(filterBy, filterValue.HasValue ? filterValue.Value.ToString() : "", filterText);
                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    List<EmployeeSkillsReportDto> empSkillsDto = techService.GetEmployeeSkillsReport(filterBy, filterValue.HasValue ? filterValue.Value.ToString() : "", filterText, RecordsPerPage, page);
                    var empReport = Mapper.Map<List<EmployeeSkillsReportDto>, List<EmployeeSkillsReportModel>>(empSkillsDto);
                    foreach (var model in empReport)
                    {
                        float overallExp = 0;
                        if (model.PastExperience.HasValue)
                        {
                            overallExp = model.PastExperience.Value + ((float)DateTime.Today.Subtract(model.DateOfJoin).TotalDays / 365);
                        }
                        else
                        {
                            overallExp = (float)DateTime.Today.Subtract(model.DateOfJoin).TotalDays / 365;
                        }
                        model.OverallExperience = string.Format("{0:0.0}", overallExp);
                    }
                    viewModel.EmployeeSkillsReports = empReport;
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

        public FileStreamResult DownloadEmpSkillsReport(string filterBy, int? filterValue, string filterText = "", int page = 1)
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Account Name,Project Name,Billability,Primary Skills,Secondary Skills,Overall Experience,Project Manager,Reporting Manager,Allocated From,Allocated Upto,Last Working Day{Environment.NewLine}");
            try
            {
                List<EmployeeSkillsReportDto> empSkillsDto = techService.GetEmployeeSkillsReport(filterBy, filterValue.HasValue ? filterValue.Value.ToString() : "", filterText, RecordsPerPage, -1);
                foreach (EmployeeSkillsReportDto dto in empSkillsDto)
                {
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.AccountName},");
                    recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.PrimarySkills?.Replace(",", ";")},");
                    recordString.Append($"{dto.SecondarySkills?.Replace(",", ";")},");
                    float overallExp = 0;
                    if (dto.PastExperience.HasValue)
                    {
                        overallExp = dto.PastExperience.Value + ((float)DateTime.Today.Subtract(dto.DateOfJoin).TotalDays / 365);
                    }
                    else
                    {
                        overallExp = (float)DateTime.Today.Subtract(dto.DateOfJoin).TotalDays / 365;
                    }

                    recordString.Append($"{String.Format("{0:0.0}", overallExp)},");
                    recordString.Append($"{dto.ProjectManager},");
                    recordString.Append($"{dto.ReportingManager},");
                    recordString.Append($"{dto.AlloctionStartDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.AllocationEndDate?.ToString("dd/MMM/yyyy")},");
                    recordString.Append($"{dto.LastWorkingDay?.ToString("dd/MMM/yyyy")}{Environment.NewLine}");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", "Billable Allocations Report.csv");
        }

        private void InitializeEmpSkillsReportPage(EmployeeSkillsReportViewModel viewModel)
        {
            viewModel.FilterTypeDropDownItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Project Account",
                    Value = "acc"
                },
                new SelectListItem
                {
                    Text = "Project Name",
                    Value = "prj"
                },
                new SelectListItem
                {
                    Text = "Allocation Type",
                    Value = "allc"
                },
                new SelectListItem
                {
                    Text = "Employee Name",
                    Value = "emp"
                },
                new SelectListItem
                {
                    Text = "Tech. SKills",
                    Value = "techs"
                },
                new SelectListItem
                {
                    Text = "Employee ID",
                    Value = "eid"
                },
                new SelectListItem
                {
                    Text = "Total Experience",
                    Value = "tot",
                },
                new SelectListItem
                {
                    Text = "Project Manager",
                    Value = "pm"
                },
                new SelectListItem
                {
                    Text = "Reporting Manager",
                    Value = "RM"
                }
            };

            if (string.IsNullOrEmpty(viewModel.FilterBy) == false && Session["FilterValueListItems"] != null)
            {
                viewModel.FilterValueDropDownItems = (List<SelectListItem>)Session["FilterValueListItems"];
            }
        }

        [HttpPost]
        public JsonResult LoadFilterValueListForSkillsReport(string filterBy)
        {
            List<SelectListItem> filterValues = new List<SelectListItem>();
            switch (filterBy)
            {
                case "emp":
                    filterValues = GetEmployeesList();
                    break;
                case "acc":
                    filterValues = GetAllAccountsList();
                    break;
                case "prj":
                    filterValues = GetProjectsList();
                    break;
                case "allc":
                    filterValues = GetAllocationTypeList();
                    break;
                case "pm":
                case "rm":
                    filterValues = GetAllManagersList();
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

        #region Private Methods

        private List<SelectListItem> GetAllManagersList()
        {
            List<EmployeeDto> employees = empService.GetAllManagers();

            List<SelectListItem> empDDList = (from e in employees
                                              select new SelectListItem
                                              {
                                                  Text = $"{e.FirstName} {e.LastName}",
                                                  Value = e.EmployeeEntryID.ToString()
                                              }).OrderBy(i => i.Text).ToList();

            ViewBag.ProjectManagerListItems = empDDList;
            return empDDList;
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
            //pmList.Insert(0, new SelectListItem { Text = "All Employees", Value = "alle" });
            return pmList;
        }

        private List<SelectListItem> GetProjectsList()
        {
            IEnumerable<ProjectDto> projects = projectService.GetAll();

            List<SelectListItem> projectList = (from p in projects
                                                orderby p.ProjectCode
                                                select new SelectListItem
                                                {
                                                    Text = $"{p.ProjectCode}-{p.ProjectName}",
                                                    Value = p.ProjectID.ToString()
                                                }).OrderBy(i => i.Text).ToList();

            return projectList;
        }

        private List<SelectListItem> GetAllocationTypeList()
        {
            IEnumerable<DropDownSubCategoryDto> buList = subCategoryService.GetSubCategories((int)CategoryType.UtilizationCode);

            List<SelectListItem> allocationTypeItems = (from c in buList
                                                        orderby c.SubCategoryName
                                                        select new SelectListItem
                                                        {
                                                            Text = c.SubCategoryName,
                                                            Value = c.SubCategoryID.ToString()
                                                        }).ToList();
            return allocationTypeItems;
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

        private List<SelectListItem> GetOtherDropDownItems()
        {
            IEnumerable<DropDownSubCategoryDto> buList = subCategoryService.GetSubCategories((int)CategoryType.UtilizationCode);

            List<SelectListItem> projectTypeItems = (from c in buList
                                                     orderby c.SubCategoryName
                                                     where c.CategoryID == (int)CategoryType.UtilizationCode
                                                     select new SelectListItem
                                                     {
                                                         Text = c.SubCategoryName,
                                                         Value = c.SubCategoryID.ToString()
                                                     }).ToList();
            projectTypeItems.Add(new SelectListItem
            {
                Text = "Not Allocated Yet (Delivery)",
                Value = "-1",
            });

            projectTypeItems.Add(new SelectListItem
            {
                Text = "Not Allocated Yet (BD/BO)",
                Value = "-2",
            });

            projectTypeItems.Add(new SelectListItem
            {
                Text = "Bench (Available)",
                Value = "-5",
            });

            projectTypeItems.Add(new SelectListItem
            {
                Text = "Bench (Earmarked)",
                Value = "-6",
            });

            return projectTypeItems;
        }

        private List<SelectListItem> GetFilterValueListItems(string filterType)
        {
            List<SelectListItem> filterValues = new List<SelectListItem>();
            switch (filterType?.ToLower())
            {
                case "all":
                    filterValues.Add(new SelectListItem
                    {
                        Text = "For All Resources",
                        Value = "all"
                    });
                    break;
                case "emp":
                    filterValues = GetEmployeesList();
                    break;
                case "prj":
                    filterValues = GetProjectsList();
                    break;
                case "acc":
                    filterValues = GetAllAccountsList();
                    break;
                case "alt":
                    filterValues = GetOtherDropDownItems();
                    break;
            }

            return filterValues;
        }

        #endregion
    }
}