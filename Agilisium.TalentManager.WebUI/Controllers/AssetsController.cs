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
    public class AssetsController : Controller
    {
        private readonly IDropDownSubCategoryService subCategoryService;
        private readonly IEmployeeTechService techService;

        public AssetsController(IDropDownSubCategoryService subCategoryService, IEmployeeTechService techService)
        {
            this.subCategoryService = subCategoryService;
            this.techService = techService;
        }

        public ActionResult Start()
        {
            return View();
        }

        public ActionResult Index(string eid = "")
        {
            EmpAssetDetailModel assetViewModel = new EmpAssetDetailModel();

            try
            {
                if (string.IsNullOrWhiteSpace(eid))
                {
                    TempData["WarningMessage"] = "We need your Employee ID to start with.";
                    return RedirectToAction("Start");
                }
                LoadDropDownItems();
                assetViewModel = GetEmployeeDetail(eid);
            }
            catch (Exception exp)
            {
                TempData["ErrorMessage"] = exp.Message;
            }
            return View(assetViewModel);
        }

        [HttpPost]
        public ActionResult Index(EmpAssetDetailModel model)
        {
            try
            {
                EmpAssetDetailDto assetDetail = Mapper.Map<EmpAssetDetailModel, EmpAssetDetailDto>(model);
                techService.UpdateEmployeeDetails(assetDetail);
            }
            catch (Exception exp)
            {
                TempData["ErrorMessage"] = exp.Message;
            }
            return RedirectToAction("UpdateEmpSkills", new { eeid = model.EmployeeEntryID, eid = model.EmployeeID });
        }

        public ActionResult UpdateEmpSkills(int eeid, string eid = "")
        {
            EmpAssetDetailViewModel assetViewModel = new EmpAssetDetailViewModel();
            try
            {

                LoadDropDownItems();
                assetViewModel.EmployeeEntryID = eeid;
                assetViewModel.EmployeeID = eid;
                assetViewModel.EmployeeSkills = LoadEmployeeSkills(eeid);
                assetViewModel.AvailableSkills = LoadTechSkills(assetViewModel.EmployeeSkills);
                assetViewModel.SkillCategories = LoadSkillCategories();
            }
            catch (Exception exp)
            {
                TempData["ErrorMessage"] = exp.Message;
            }
            return View(assetViewModel);
        }

        public ActionResult AddEmpSkill(string sid, string eid, int eeid)
        {
            EmployeeSkillModel skillModel = new EmployeeSkillModel();

            try
            {
                LoadDropDownItems();
                skillModel.EmployeeEntryID = eeid;
                skillModel.EmployeeID = eid;
                int.TryParse(sid, out int skillID);
                TechSkillDto techSkill = techService.GetTechSkillByID(skillID);
                if (techSkill == null)
                {
                    TempData["ErrorMessage"] = "Sorry for the inconvience. We couldn't retrieve the technology details.";
                }
                else
                {
                    skillModel.SkillCategoryID = techSkill.TechSkillCategoryID;
                    skillModel.TechSkillID = techSkill.TechSkillID;
                    skillModel.TechSkill = techSkill.TechSkillName;
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Sorry for the inconvience. We couldn't retrieve the technology details.";
            }
            return View(skillModel);
        }

        [HttpPost]
        public ActionResult AddEmpSkill(EmployeeSkillModel skillModel)
        {
            try
            {
                EmployeeSkillDto skillDto = Mapper.Map<EmployeeSkillModel, EmployeeSkillDto>(skillModel);
                techService.AddEmpSkill(skillDto);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Sorry for the inconvience. We couldn't retrieve the technology details.";
            }
            return RedirectToAction("UpdateEmpSkills", new { eeid = skillModel.EmployeeEntryID, eid = skillModel.EmployeeID });
        }

        public ActionResult ModifyEmpSkill(int id, int eeid, string eid = "")
        {
            EmployeeSkillModel skillModel = new EmployeeSkillModel();
            skillModel.EmployeeID = eid;
            try
            {
                if (techService.DoesEmployeeSkillExist(id) == false)
                {
                    TempData["ErrorMessage"] = "We couldn't find the employee skill details";
                    return RedirectToAction("UpdateEmpSkills", new { eeid, eid });
                }

                EmployeeSkillDto skillDto = techService.GetEmployeeSkillByID(id);
                skillModel = Mapper.Map<EmployeeSkillDto, EmployeeSkillModel>(skillDto);
                LoadDropDownItems();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error has occured while reading your technical skills";
            }
            return View(skillModel);
        }

        [HttpPost]
        public ActionResult ModifyEmpSkill(EmployeeSkillModel skillModel, string eid = "")
        {
            try
            {
                EmployeeSkillDto skillDto = Mapper.Map<EmployeeSkillModel, EmployeeSkillDto>(skillModel);
                techService.UpdateEmpSkill(skillDto);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error has occured while updating the technology skills";
            }
            return RedirectToAction("UpdateEmpSkills", new { eeid = skillModel.EmployeeEntryID, eid });
        }

        public ActionResult RemoveEmpSkill(int sid, int eeid, string eid = "")
        {
            try
            {
                EmployeeSkillDto employeeSkillDto = new EmployeeSkillDto { EmployeeSkillID = sid };
                techService.DeleteEmpTechSkill(employeeSkillDto);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Sorry for the inconvience. We couldn't retrieve the technology details.";
            }
            return RedirectToAction("UpdateEmpSkills", new { eeid, eid });
        }

        private void LoadDropDownItems()
        {
            List<DropDownSubCategoryDto> buList = subCategoryService.GetSubCategories((int)CategoryType.VisaCategory).ToList();

            List<SelectListItem> visaListItems = (from c in buList
                                                  orderby c.SubCategoryName
                                                  select new SelectListItem
                                                  {
                                                      Text = c.SubCategoryName,
                                                      Value = c.SubCategoryID.ToString()
                                                  }).ToList();
            ViewBag.VisaTypeListItems = visaListItems;


            List<DropDownSubCategoryDto> locationsList = subCategoryService.GetSubCategories((int)CategoryType.WorkLocation).ToList();

            List<SelectListItem> locListItems = (from c in locationsList
                                                 orderby c.SubCategoryName
                                                 select new SelectListItem
                                                 {
                                                     Text = c.SubCategoryName,
                                                     Value = c.SubCategoryID.ToString()
                                                 }).ToList();
            ViewBag.LocationsListItems = locListItems;

            List<DropDownSubCategoryDto> ratingList = subCategoryService.GetSubCategories((int)CategoryType.TechnologyRating).ToList();

            List<SelectListItem> ratingListItems = (from c in ratingList
                                                    orderby c.SubCategoryName
                                                    select new SelectListItem
                                                    {
                                                        Text = c.SubCategoryName,
                                                        Value = c.SubCategoryID.ToString()
                                                    }).ToList();
            ViewBag.RatingsListItems = ratingListItems;

        }

        private EmpAssetDetailModel GetEmployeeDetail(string eid)
        {
            EmpAssetDetailDto empAssetDto = techService.GetByEmployeeID(eid);
            return Mapper.Map<EmpAssetDetailDto, EmpAssetDetailModel>(empAssetDto);
        }

        private List<EmployeeSkillModel> LoadEmployeeSkills(int eeid)
        {
            List<EmployeeSkillDto> empSkillsDto = techService.GetAllEmployeeSkills(eeid);
            return Mapper.Map<List<EmployeeSkillDto>, List<EmployeeSkillModel>>(empSkillsDto);
        }

        private List<TechSkillModel> LoadTechSkills(List<EmployeeSkillModel> empSkills)
        {
            List<TechSkillModel> techSkillsModel = null;
            if (Session["AllTechSkills"] == null)
            {
                List<TechSkillDto> empSkillsDto = techService.GetAllAvailableTechSkills();
                techSkillsModel = Mapper.Map<List<TechSkillDto>, List<TechSkillModel>>(empSkillsDto);
                Session["AllTechSkills"] = techSkillsModel;
            }
            techSkillsModel = (List<TechSkillModel>)Session["AllTechSkills"];

            List<TechSkillModel> availableSkills = new List<TechSkillModel>();
            foreach (TechSkillModel skill in techSkillsModel)
            {
                if (empSkills.Any(e => e.TechSkillID == skill.TechSkillID))
                {
                    continue;
                }

                availableSkills.Add(new TechSkillModel
                {
                    TechSkillCategoryID = skill.TechSkillCategoryID,
                    TechSkillID = skill.TechSkillID,
                    TechSkillName = skill.TechSkillName
                });
            }
            return availableSkills;
        }

        private List<TechSkillCategoryModel> LoadSkillCategories()
        {
            List<TechSkillCategoryModel> categoriesModel = null;

            if (Session["AllTechSkillCategories"] == null)
            {
                List<TechSkillCategoryDto> categories = techService.GetAllAvailableSkillCategories();
                categoriesModel = Mapper.Map<List<TechSkillCategoryDto>, List<TechSkillCategoryModel>>(categories);
                Session["AllTechSkillCategories"] = categoriesModel;
            }
            categoriesModel = (List<TechSkillCategoryModel>)Session["AllTechSkillCategories"];
            return categoriesModel;
        }


    }
}