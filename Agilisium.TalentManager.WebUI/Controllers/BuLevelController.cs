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
    public class BuLevelController : BaseController
    {
        private readonly IBuLevelService BuLevelService;
        private readonly IDropDownSubCategoryService subCategoryService;

        public BuLevelController(IBuLevelService BuLevelService, IDropDownSubCategoryService subCategoryService)
        {
            this.BuLevelService = BuLevelService;
            this.subCategoryService = subCategoryService;
        }

        // GET: BuLevel
        public ActionResult List(int page = 1)
        {
            BuLevelViewModel viewModel = new BuLevelViewModel();

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = BuLevelService.TotalRecordsCount(),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.BuLevels = GetBuLevels(page);
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

        // GET: BuLevel/Create
        public ActionResult Create()
        {
            try
            {
                LoadBusinessUnitItems();
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(new BuLevelModel());
        }

        // POST: BuLevel/Create
        [HttpPost]
        public ActionResult Create(BuLevelModel BuLevel)
        {
            try
            {
                LoadBusinessUnitItems();

                if (ModelState.IsValid)
                {
                    if (BuLevelService.Exists(BuLevel.ItemName))
                    {
                        DisplayWarningMessage($"The Level Name '{BuLevel.ItemName}' is duplicate");
                        return View(BuLevel);
                    }
                    BuLevelDto BuLevelModel = Mapper.Map<BuLevelModel, BuLevelDto>(BuLevel);
                    BuLevelService.Add(BuLevelModel);
                    DisplaySuccessMessage($"New BU Level '{BuLevel.ItemName}' has been stored successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(BuLevel);
        }

        // GET: BuLevel/Edit/5
        public ActionResult Edit(int? id)
        {
            BuLevelModel BuLevel = new BuLevelModel();

            LoadBusinessUnitItems();

            try
            {
                if (!id.HasValue)
                {
                    DisplayWarningMessage("Looks like, the ID is missing in your request");
                    return RedirectToAction("List");
                }

                if (!BuLevelService.Exists(id.Value))
                {
                    DisplayWarningMessage($"Sorry, We couldn't find the Level with ID: {id.Value}");
                    return RedirectToAction("List");
                }

                BuLevelDto BuLevelDto = BuLevelService.GetlevelItem(id.Value);
                BuLevel = Mapper.Map<BuLevelDto, BuLevelModel>(BuLevelDto);
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }

            return View(BuLevel);
        }

        // POST: BuLevel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BuLevelModel BuLevel)
        {
            try
            {
                LoadBusinessUnitItems();

                if (ModelState.IsValid)
                {
                    if (BuLevelService.Exists(BuLevel.ItemName, BuLevel.ItemEntryID))
                    {
                        DisplayWarningMessage($"Level Name '{BuLevel.ItemName}' is duplicate");
                        return View(BuLevel);
                    }

                    BuLevelDto BuLevelModel = Mapper.Map<BuLevelModel, BuLevelDto>(BuLevel);
                    BuLevelService.Update(BuLevelModel);
                    DisplaySuccessMessage($"BU Level '{BuLevel.ItemName}' details have been modified successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(BuLevel);
        }

        // POST: BuLevel/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the ID is missing in your request");
                return RedirectToAction("List");
            }

            try
            {
                if (BuLevelService.CanBeDeleted(id.Value) == false)
                {
                    DisplayWarningMessage("There are some dependencies with this BU Level. So, you can't delete this for now");
                    return RedirectToAction("List");
                }

                BuLevelService.Delete(new BuLevelDto { ItemEntryID = id.Value });
                DisplaySuccessMessage("BU Level has been deleted successfully");
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult GetBuLevelsByBuID(int buID)
        {
            IEnumerable<BuLevelDto> BuLevels = BuLevelService.GetAllByBU(buID).ToList();

            List<SelectListItem> BuLevelItems = (from p in BuLevels
                                                  select new SelectListItem
                                                  {
                                                      Value = p.ItemEntryID.ToString(),
                                                      Text = p.ItemName
                                                  }).ToList();
            return Json(BuLevelItems);
        }

        private IEnumerable<BuLevelModel> GetBuLevels(int pageNo)
        {
            IEnumerable<BuLevelDto> BuLevels = BuLevelService.GetAll(RecordsPerPage, pageNo);
            IEnumerable<BuLevelModel> BuLevelModels = Mapper.Map<IEnumerable<BuLevelDto>, IEnumerable<BuLevelModel>>(BuLevels);
            return BuLevelModels;
        }

        private void LoadBusinessUnitItems()
        {
            IEnumerable<DropDownSubCategoryDto> buList = subCategoryService.GetSubCategories((int)CategoryType.BusinessUnit);

            List<SelectListItem> buListItems = (from c in buList
                                                orderby c.SubCategoryName
                                                where c.CategoryID == (int)CategoryType.BusinessUnit
                                                select new SelectListItem
                                                {
                                                    Text = c.SubCategoryName,
                                                    Value = c.SubCategoryID.ToString()
                                                }).ToList();

            ViewBag.BuListItems = buListItems;
        }
    }
}
