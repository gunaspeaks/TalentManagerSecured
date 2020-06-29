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
    public class EmployeeController : BaseController
    {
        private const string NO_VALID_VISA = "No Valid Visa";

        private readonly IEmployeeService empService;
        private readonly IDropDownSubCategoryService subCategoryService;
        private readonly IBuLevelService buLevelService;
        private readonly IResourceLevelService resLevelService;
        private readonly ICertificationService certService;
        private readonly IProjectAccountService accService;
        private readonly IProjectService projectService;

        public EmployeeController(IEmployeeService empService, IDropDownSubCategoryService subCategoryService, IBuLevelService buLevelService,
            IResourceLevelService resLevelService, ICertificationService certService, IProjectAccountService accService,
            IProjectService projectService)
        {
            this.empService = empService;
            this.subCategoryService = subCategoryService;
            this.buLevelService = buLevelService;
            this.resLevelService = resLevelService;
            this.certService = certService;
            this.accService = accService;
            this.projectService = projectService;
        }

        // GET: Employee
        public ActionResult List(string searchText, int page = 1)
        {
            EmployeeViewModel viewModel = new EmployeeViewModel
            {
                SearchText = searchText
            };

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = empService.TotalRecordsCount(searchText),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Employees = GetEmployees(searchText, page);
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

        // GET: Employee
        public ActionResult PastEmployeeList(int page = 1)
        {
            EmployeeViewModel viewModel = new EmployeeViewModel();

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = empService.GetPastEmployeesCount(),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Employees = GetPastEmployees(page);
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

        // GET: Employee
        public ActionResult PracticeWiseList(int pid, int page = 1)
        {
            EmployeeViewModel viewModel = new EmployeeViewModel
            {
                SearchText = "",
                PID = pid
            };

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = empService.PracticeWiseRecordsCount(pid),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Employees = GetPracticeWiseEmployees(pid, page);
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

        // GET: Employee
        public ActionResult SubPracticeWiseList(int sid, int page = 1)
        {
            EmployeeViewModel viewModel = new EmployeeViewModel
            {
                SearchText = "",
                SID = sid
            };

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = empService.SubPracticeWiseRecordsCount(sid),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.Employees = GetSubPracticeWiseEmployees(sid, page);
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

        // GET: Employe/Create
        [Authorize(Roles = "Human Resource, Super Admin, Admin")]
        public ActionResult Create()
        {
            EmployeeModel emp = new EmployeeModel()
            {
                DateOfJoin = DateTime.Now,
            };

            try
            {
                InitializePageData(-1);
                emp.VisaCategoryID = int.Parse(ViewBag.NoVisaCategoryID);
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(emp);
        }

        // POST: Employe/Create
        [HttpPost]
        [Authorize(Roles = "Human Resource, Super Admin, Admin")]
        public ActionResult Create(EmployeeModel employee)
        {
            try
            {
                InitializePageData(-1);

                if (ModelState.IsValid)
                {
                    if (!IsValidEmployeeData(employee)) return View(employee);

                    if (empService.IsDuplicateName(employee.FirstName, employee.LastName))
                    {
                        DisplayWarningMessage("There is already an Employee with the same First and Last Name");
                        return View(employee);
                    }

                    if (empService.IsDuplicateEmployeeID(employee.EmployeeID))
                    {
                        DisplayWarningMessage("This Employee ID is already exists");
                        return View(employee);
                    }
                    employee.PrimarySkills = employee.PrimarySkills?.Replace(",", ";");
                    employee.SecondarySkills = employee.SecondarySkills?.Replace(",", ";");
                    EmployeeDto employeeDto = Mapper.Map<EmployeeModel, EmployeeDto>(employee);
                    empService.Create(employeeDto);
                    DisplaySuccessMessage("New Employee details have been stored successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(employee);
        }

        private bool IsValidEmployeeData(EmployeeModel employee)
        {
            if (employee.Level1ID.HasValue == false || (employee.Level1ID.HasValue && employee.Level1ID <= 0))
            {
                DisplayWarningMessage("Please select a value for Level 1 field");
                return false;
            }

            if (employee.BusinessUnitID == (int)BusinessUnitType.BusinessDelevopment &&
                employee.Level2ID.HasValue == false || (employee.Level2ID.HasValue && employee.Level2ID <= 0))
            {
                DisplayWarningMessage("Please select a value for Level 2 field");
                return false;
            }

            if (employee.BusinessUnitID == (int)BusinessUnitType.Delivery)
            {
                if (employee.Level1ID.HasValue == false || (employee.Level1ID.HasValue && employee.Level1ID <= 0))
                {
                    DisplayWarningMessage("Please select an Account from Level 1 field");
                    return false;
                }

                if (employee.Level2ID.HasValue == false || (employee.Level2ID.HasValue && employee.Level2ID <= 0))
                {
                    DisplayWarningMessage("Please select a Project from Level 2 field");
                    return false;
                }

                if (employee.Level3ID.HasValue == false || (employee.Level3ID.HasValue && employee.Level3ID <= 0))
                {
                    DisplayWarningMessage("Please select a Billable Type from Level 3 field");
                    return false;
                }

                if (employee.Level3ID == (int)BillabilityType.NonBillable &&
                    employee.Level4ID.HasValue == false || (employee.Level4ID.HasValue && employee.Level4ID <= 0))
                {
                    DisplayWarningMessage("Please select a value for Level 4 field");
                    return false;
                }

                if (employee.Level3ID == (int)BillabilityType.NonBillable &&
                    employee.Level5ID.HasValue == false || (employee.Level5ID.HasValue && employee.Level5ID <= 0))
                {
                    DisplayWarningMessage("Please select a value for Level 5 field");
                    return false;
                }
            }

            return true;
        }

        // GET: Employe/Edit/5
        [Authorize(Roles = "Human Resource, Super Admin, Admin")]
        public ActionResult Edit(int? id)
        {
            EmployeeModel empModel = new EmployeeModel();
            try
            {
                InitializePageData(id ?? -1);

                if (!id.HasValue)
                {
                    DisplayWarningMessage("Looks like, the employee ID is missing in your request");
                    return View(empModel);
                }

                if (!empService.Exists(id.Value))
                {
                    DisplayWarningMessage($"Sorry, we couldn't find the Employee details for the given ID: {id.Value}");
                    return View(empModel);
                }

                EmployeeDto emp = empService.GetEmployee(id.Value);
                LoadListItemsBasedOnEmployeeData(emp);
                empModel = Mapper.Map<EmployeeDto, EmployeeModel>(emp);
                if (string.IsNullOrWhiteSpace(empModel.TravelledCountries))
                {
                    empModel.TravelledCountries = "None";
                }

                if (empModel.VisaCategoryID.HasValue == false)
                {
                    empModel.VisaCategoryID = int.Parse(ViewBag.NoVisaCategoryID);
                }
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }
            return View(empModel);
        }

        private void LoadListItemsBasedOnEmployeeData(EmployeeDto employee)
        {
            IEnumerable<BuLevelDto> buLevelItems = buLevelService.GetAllByBU(employee.BusinessUnitID);
            List<SelectListItem> level1Items = new List<SelectListItem>();

            if (employee.BusinessUnitID == (int)BusinessUnitType.BusinessDelevopment)
            {
                ViewBag.Level1ListItems = (from a in buLevelItems
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                int salesLevel = buLevelItems.Count() > 0 ? buLevelItems.FirstOrDefault(a => a.ItemName == "Sales").ItemEntryID : -1;
                List<ResourceLevelDto> resLevels = resLevelService.GetAllByLevel(salesLevel).ToList();
                ViewBag.Level2ListItems = (from a in resLevels
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level3ListItems = (from a in resLevels
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level4ListItems = (from a in resLevels
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level5ListItems = (from a in resLevels
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

            }
            if (employee.BusinessUnitID == (int)BusinessUnitType.Operations
                || employee.BusinessUnitID == (int)BusinessUnitType.RMG)
            {
                ViewBag.Level1ListItems = (from a in buLevelItems
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level2ListItems = (from a in buLevelItems
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level3ListItems = (from a in buLevelItems
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level4ListItems = (from a in buLevelItems
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

                ViewBag.Level5ListItems = (from a in buLevelItems
                                           orderby a.ItemName
                                           select new SelectListItem
                                           {
                                               Text = a.ItemName,
                                               Value = a.ItemEntryID.ToString(),
                                           }).ToList();

            }
            else if (employee.BusinessUnitID == (int)BusinessUnitType.Delivery)
            {
                List<ProjectAccountDto> accounts = accService.GetAll();
                ViewBag.Level1ListItems = (from a in accounts
                                           orderby a.AccountName
                                           select new SelectListItem
                                           {
                                               Text = a.AccountName,
                                               Value = a.AccountID.ToString(),
                                           }).ToList();

                if (employee.Level1ID.HasValue)
                {
                    List<ProjectDto> projects = projectService.GetAllByAccount(employee.Level1ID.Value).ToList();
                    ViewBag.Level2ListItems = (from a in projects
                                               orderby a.ProjectName, a.ProjectCode
                                               select new SelectListItem
                                               {
                                                   Text = a.ProjectName + " (" + a.ProjectCode + ")",
                                                   Value = a.ProjectID.ToString(),
                                               }).ToList();
                }

                List<DropDownSubCategoryDto> subCategories = subCategoryService.GetSubCategories((int)CategoryType.BillabilityType).ToList();
                ViewBag.Level3ListItems = (from a in subCategories
                                           orderby a.SubCategoryName
                                           select new SelectListItem
                                           {
                                               Text = a.SubCategoryName,
                                               Value = a.SubCategoryID.ToString(),
                                           }).ToList();

                if (employee.Level3ID == (int)BillabilityType.Billable)
                {
                    ViewBag.Level4ListItems = (from a in subCategories
                                               orderby a.SubCategoryName
                                               select new SelectListItem
                                               {
                                                   Text = a.SubCategoryName,
                                                   Value = a.SubCategoryID.ToString(),
                                               }).ToList();

                    ViewBag.Level5ListItems = (from a in subCategories
                                               orderby a.SubCategoryName
                                               select new SelectListItem
                                               {
                                                   Text = a.SubCategoryName,
                                                   Value = a.SubCategoryID.ToString(),
                                               }).ToList();
                }
                else
                {
                    List<DropDownSubCategoryDto> level4SubCategories = subCategoryService.GetSubCategories((int)CategoryType.Level4_NonBillableItems).ToList();
                    ViewBag.Level4ListItems = (from a in level4SubCategories
                                               orderby a.SubCategoryName
                                               select new SelectListItem
                                               {
                                                   Text = a.SubCategoryName,
                                                   Value = a.SubCategoryID.ToString(),
                                               }).ToList();

                    List<DropDownSubCategoryDto> level5SubCategories = subCategoryService.GetSubCategories((int)CategoryType.Level5_NonBillableItems).ToList();
                    ViewBag.Level5ListItems = (from a in level5SubCategories
                                               orderby a.SubCategoryName
                                               select new SelectListItem
                                               {
                                                   Text = a.SubCategoryName,
                                                   Value = a.SubCategoryID.ToString(),
                                               }).ToList();
                }
            }
            else
            {
                // reserved
            }
        }

        private void LoadListItemsBasedOnEmployeeData(EmployeeModel employee)
        {
            EmployeeDto employeeDto = Mapper.Map<EmployeeModel, EmployeeDto>(employee);
            LoadListItemsBasedOnEmployeeData(employeeDto);
        }

        public ActionResult View(int? id)
        {
            EmployeeModel empModel = new EmployeeModel();

            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the employee ID is missing in your request");
                return View(empModel);
            }

            try
            {

                if (!empService.Exists(id.Value))
                {
                    DisplayWarningMessage($"Sorry, we couldn't find the Employee with ID: {id.Value}");
                    return View(empModel);
                }

                EmployeeDto emp = empService.GetEmployee(id.Value);
                empModel = Mapper.Map<EmployeeDto, EmployeeModel>(emp);
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }
            return View(empModel);
        }

        // POST: Employe/Edit/5
        [HttpPost]
        [Authorize(Roles = "Human Resource, Super Admin, Admin")]
        public ActionResult Edit(EmployeeModel employee, int? page)
        {
            try
            {
                InitializePageData(employee.EmployeeEntryID);
                LoadListItemsBasedOnEmployeeData(employee);

                if (ModelState.IsValid)
                {
                    if (!IsValidEmployeeData(employee)) return View(employee);

                    if (empService.IsDuplicateName(employee.EmployeeEntryID, employee.FirstName, employee.LastName))
                    {
                        DisplayWarningMessage("There is already an Employee with the same First and Last Name");
                        return View(employee);
                    }

                    if (empService.IsDuplicateEmployeeID(employee.EmployeeEntryID, employee.EmployeeID))
                    {
                        DisplayWarningMessage("This Employee ID is already exists");
                        return View(employee);
                    }

                    employee.PrimarySkills = employee.PrimarySkills?.Replace(",", ";");
                    employee.SecondarySkills = employee.SecondarySkills?.Replace(",", ";");
                    EmployeeDto employeeDto = Mapper.Map<EmployeeModel, EmployeeDto>(employee);
                    empService.Update(employeeDto);
                    DisplaySuccessMessage("Employee details have been Updated successfully");
                    return RedirectToAction("List", new { page });
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(employee);
        }

        // GET: Employe/Delete/5
        [Authorize(Roles = "Human Resource, Super Admin, Admin")]
        public ActionResult Delete(int? id, int? page)
        {
            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the employee ID is missing in your request");
                return RedirectToAction("List", new { page });
            }

            try
            {
                empService.Delete(new EmployeeDto { EmployeeEntryID = id.Value });
                DisplaySuccessMessage("Employee details have been deleted successfully");
                return RedirectToAction("List", new { page });
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
                return RedirectToAction("List", new { page });
            }
        }

        public ActionResult ChangeReportingManager()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSubPracticeList(int id)
        {
            return Json("");
        }

        [HttpPost]
        public JsonResult GenerateNewEmployeeID(int id)
        {
            return Json(empService.GenerateNewEmployeeID(id));
        }

        [HttpPost]
        public JsonResult GetEmployeeDetails(int id)
        {
            EmployeeDto emp = empService.GetEmployee(id);
            return Json(emp);
        }

        [HttpPost]
        public JsonResult GetEmailID(int id)
        {
            return Json(empService.GetEmailID(id));
        }

        [Authorize(Roles = "Human Resource, Super Admin")]
        public FileStreamResult DownloadAllEmployees(string filterType, string filterValue)
        {
            StringBuilder recordString = new StringBuilder($"Employee ID,Employee Name,Employee Type,Business Unit," +
                $"Primary Skills,Visa Category,Visa Valid Upto,Overall Experience," +
                $"Project Manager,Allocated From,Allocated Upto,Reporting Manager{Environment.NewLine}");
            try
            {
                List<EmpAndAllocationDto> employees = empService.GetAllEmployeesWithAllocationDetails();
                foreach (EmpAndAllocationDto dto in employees)
                {
                    //
                    recordString.Append($"{dto.EmployeeID},");
                    recordString.Append($"{dto.EmployeeName},");
                    recordString.Append($"{dto.EmploymentType},");
                    recordString.Append($"{dto.BusinessUnit},");
                    recordString.Append($"{dto.PrimarySkills?.Replace(",", ";")},");
                    //recordString.Append($"{dto.SecondarySkills?.Replace(",", ";")},");
                    //  level 1 to 5
                    //recordString.Append($"{dto.TechnicalRank},");
                    //recordString.Append($"{dto.StrengthArea},");
                    recordString.Append($"{dto.VisaCategory},");
                    recordString.Append($"{dto.VisaValidUpto?.ToString("dd-MMM-yyyy")},");
                    recordString.Append($"{dto.OveralExperience},");
                    //recordString.Append($"{dto.AccountName},");
                    //recordString.Append($"{dto.ProjectName},");
                    recordString.Append($"{dto.ProjectManager},");
                    //recordString.Append($"{dto.ProjectType},");
                    //recordString.Append($"{dto.AllocationType},");
                    recordString.Append($"{dto.AllocationStartDate?.ToString("dd-MMM-yyyy")},");
                    recordString.Append($"{dto.AllocationEndDate?.ToString("dd-MMM-yyyy")},");
                    recordString.Append($"{dto.ReportingManager}{Environment.NewLine}");
                }

            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            byte[] byteArr = Encoding.ASCII.GetBytes(recordString.ToString());
            MemoryStream stream = new MemoryStream(byteArr);
            return File(stream, "application/vnd.ms-excel", $"Employees As On {DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}.csv");
        }

        public ActionResult AddCertification(int? cid, int? eid)
        {
            EmpCertificationModel certModel = new EmpCertificationModel();

            try
            {
                if (!cid.HasValue)
                {
                    DisplayWarningMessage("Looks like, the Certification ID is invalid");
                    return View(certModel);
                }
                certModel.CertificationID = cid.Value;
                CertificationDto cert = certService.GetByID(certModel.CertificationID);
                certModel.CertificationName = cert?.Name;
                certModel.ShortName = cert?.ShortName;

                if (!eid.HasValue)
                {
                    DisplayWarningMessage("Looks like, the Employee ID is invalid");
                    return View(certModel);
                }
                certModel.EmployeeID = eid.Value;
                EmployeeDto emp = empService.GetEmployee(eid.Value);
                certModel.EmployeeName = $"{emp?.FirstName} {emp?.LastName}";
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(certModel);
        }

        [HttpPost]
        public ActionResult AddCertification(EmpCertificationModel certificationModel)
        {
            try
            {
                EmpCertificationDto certDto = Mapper.Map<EmpCertificationModel, EmpCertificationDto>(certificationModel);
                empService.AddCertification(certDto);
                DisplaySuccessMessage("Certification added successfully");
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return RedirectToAction("Certifications", new { eid = certificationModel.EmployeeID });
        }

        public ActionResult Certifications(string eid)
        {
            EmpCertificationsViewModel viewModel = new EmpCertificationsViewModel();

            try
            {
                int empID = -1;
                List<CertificationDto> allCerts = certService.GetAll();
                viewModel.AvailableCertifications = Mapper.Map<List<CertificationDto>, List<CertificationModel>>(allCerts);

                if (!string.IsNullOrWhiteSpace(eid))
                {
                    int.TryParse(eid, out empID);
                    List<EmpCertificationDto> aquiredCerts = empService.GetCertificationsByEmployeeID(empID);
                    viewModel.AquiredCertifications = Mapper.Map<List<EmpCertificationDto>, List<EmpCertificationModel>>(aquiredCerts);

                    foreach (EmpCertificationModel ac in viewModel.AquiredCertifications)
                    {
                        viewModel.AvailableCertifications.RemoveAll(c => c.CertificationID == ac.CertificationID);
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(viewModel);
        }

        public ActionResult DeleteCertification(int? id, int? eid)
        {
            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the Certification ID is missing in your request");
                return RedirectToAction("Certifications", new { eid = eid.Value });
            }

            try
            {
                empService.DeleteCertification(new EmpCertificationDto { EntryID = id.Value, EmployeeID = eid.Value });
                DisplaySuccessMessage("Certifications have been removed for the employee successfully");
                return RedirectToAction("Certifications", new { eid = eid.Value });
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
                return RedirectToAction("Certifications", new { eid = eid.Value });
            }
        }

        private IEnumerable<EmployeeModel> GetEmployees(string searchText, int pageNo = 1)
        {
            IEnumerable<EmployeeDto> employees = empService.GetAllEmployees(searchText, RecordsPerPage, pageNo);
            IEnumerable<EmployeeModel> employeeModels = Mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<EmployeeModel>>(employees);

            return employeeModels;
        }

        private IEnumerable<EmployeeModel> GetPastEmployees(int pageNo)
        {
            IEnumerable<EmployeeDto> employees = empService.GetAllPastEmployees(RecordsPerPage, pageNo);
            IEnumerable<EmployeeModel> employeeModels = Mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<EmployeeModel>>(employees);

            return employeeModels;
        }

        private IEnumerable<EmployeeModel> GetSubPracticeWiseEmployees(int subPracticeID, int pageNo = 1)
        {
            IEnumerable<EmployeeDto> employees = empService.GetAllBySubPractice(subPracticeID, RecordsPerPage, pageNo);
            IEnumerable<EmployeeModel> employeeModels = Mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<EmployeeModel>>(employees);

            return employeeModels;
        }

        private IEnumerable<EmployeeModel> GetPracticeWiseEmployees(int practiceID, int pageNo = 1)
        {
            IEnumerable<EmployeeDto> employees = empService.GetAllByPractice(practiceID, RecordsPerPage, pageNo);
            IEnumerable<EmployeeModel> employeeModels = Mapper.Map<IEnumerable<EmployeeDto>, IEnumerable<EmployeeModel>>(employees);

            return employeeModels;
        }

        private void InitializePageData(int employeeID = -1)
        {
            ViewBag.IsNewEntry = true;
            ViewBag.Level1ListItems = new List<SelectListItem>();
            ViewBag.Level2ListItems = new List<SelectListItem>();
            ViewBag.Level3ListItems = new List<SelectListItem>();
            ViewBag.Level4ListItems = new List<SelectListItem>();
            ViewBag.Level5ListItems = new List<SelectListItem>();

            GetReportingManagersList(employeeID);
            GetOtherDropDownItems();
        }

        private void GetOtherDropDownItems()
        {
            List<DropDownSubCategoryDto> buList = subCategoryService.GetAll().ToList();

            List<SelectListItem> buListItems = (from c in buList
                                                orderby c.SubCategoryName
                                                where c.CategoryID == (int)CategoryType.BusinessUnit
                                                select new SelectListItem
                                                {
                                                    Text = c.SubCategoryName,
                                                    Value = c.SubCategoryID.ToString()
                                                }).ToList();

            ViewBag.BuListItems = buListItems;

            List<SelectListItem> ucListItems = (from c in buList
                                                orderby c.SubCategoryName
                                                where c.CategoryID == (int)CategoryType.UtilizationCode
                                                select new SelectListItem
                                                {
                                                    Text = c.SubCategoryName,
                                                    Value = c.SubCategoryID.ToString()
                                                }).ToList();

            ViewBag.UtilizationTypeListItems = ucListItems;

            List<SelectListItem> empTypeList = (from c in buList
                                                orderby c.SubCategoryName
                                                where c.CategoryID == (int)CategoryType.EmploymentType
                                                select new SelectListItem
                                                {
                                                    Text = c.SubCategoryName,
                                                    Value = c.SubCategoryID.ToString()
                                                }).ToList();

            ViewBag.EmploymentTypeListItems = empTypeList;

            List<SelectListItem> visaListItems = (from c in buList
                                                  orderby c.SubCategoryName
                                                  where c.CategoryID == (int)CategoryType.VisaCategory
                                                  select new SelectListItem
                                                  {
                                                      Text = c.SubCategoryName,
                                                      Value = c.SubCategoryID.ToString()
                                                  }).ToList();
            ViewBag.NoVisaCategoryID = visaListItems.FirstOrDefault(i => i.Text == NO_VALID_VISA).Value;
            ViewBag.VisaCategoryListItems = visaListItems;

            List<SelectListItem> benchCategoryList = (from c in buList
                                                      orderby c.SubCategoryName
                                                      where c.CategoryID == (int)CategoryType.BenchCategory
                                                      select new SelectListItem
                                                      {
                                                          Text = c.SubCategoryName,
                                                          Value = c.SubCategoryID.ToString()
                                                      }).ToList();

            ViewBag.BenchCategoryListItems = benchCategoryList;

            List<SelectListItem> gradeListItems = new List<SelectListItem>();
            for (int i = 10; i >= 1; i--)
            {
                gradeListItems.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.GradeListItems = gradeListItems;

            ViewBag.EmptyListItems = new List<SelectListItem>();
        }

        private void GetReportingManagersList(int employeeID)
        {
            List<EmployeeDto> managers = empService.GetAllManagers();

            List<SelectListItem> empDDList = new List<SelectListItem>();

            if (managers != null)
            {
                empDDList = (from e in managers
                             where e.EmployeeEntryID != employeeID
                             select new SelectListItem
                             {
                                 Text = $"{e.FirstName} {e.LastName}",
                                 Value = e.EmployeeEntryID.ToString()
                             }).OrderBy(i => i.Text).ToList();
            }

            ViewBag.ReportingManagerListItems = empDDList;
        }
    }
}
