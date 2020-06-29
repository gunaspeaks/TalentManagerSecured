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
    public class RLevelController : BaseController
    {
        private readonly IResourceLevelService rLevelService;
        private readonly IBuLevelService buLevelService;

        public RLevelController(IResourceLevelService rLevelService, IBuLevelService buLevelService)
        {
            this.rLevelService = rLevelService;
            this.buLevelService = buLevelService;
        }

        // GET: ResourceLevel
        public ActionResult List(int page = 1)
        {
            ResourceLevelViewModel viewModel = new ResourceLevelViewModel();

            try
            {
                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = rLevelService.TotalRecordsCount(),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    viewModel.ResourceLevels = GetResourceLevels(page);
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

        // GET: ResourceLevel/Create
        public ActionResult Create()
        {
            try
            {
                LoadDropDownItems();
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(new ResourceLevelModel());
        }

        // POST: ResourceLevel/Create
        [HttpPost]
        public ActionResult Create(ResourceLevelModel ResourceLevel)
        {
            try
            {
                LoadDropDownItems();

                if (ModelState.IsValid)
                {
                    if (rLevelService.Exists(ResourceLevel.ItemName))
                    {
                        DisplayWarningMessage($"The Level Name '{ResourceLevel.ItemName}' is duplicate");
                        return View(ResourceLevel);
                    }
                    ResourceLevelDto ResourceLevelModel = Mapper.Map<ResourceLevelModel, ResourceLevelDto>(ResourceLevel);
                    rLevelService.Add(ResourceLevelModel);
                    DisplaySuccessMessage($"New Resource Level '{ResourceLevel.ItemName}' has been stored successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(ResourceLevel);
        }

        // GET: ResourceLevel/Edit/5
        public ActionResult Edit(int? id)
        {
            ResourceLevelModel ResourceLevel = new ResourceLevelModel();

            LoadDropDownItems();

            try
            {
                if (!id.HasValue)
                {
                    DisplayWarningMessage("Looks like, the ID is missing in your request");
                    return RedirectToAction("List");
                }

                if (!rLevelService.Exists(id.Value))
                {
                    DisplayWarningMessage($"Sorry, We couldn't find the Level with ID: {id.Value}");
                    return RedirectToAction("List");
                }

                ResourceLevelDto ResourceLevelDto = rLevelService.GetLevelItem(id.Value);
                ResourceLevel = Mapper.Map<ResourceLevelDto, ResourceLevelModel>(ResourceLevelDto);
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }

            return View(ResourceLevel);
        }

        // POST: ResourceLevel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ResourceLevelModel ResourceLevel)
        {
            try
            {
                LoadDropDownItems();

                if (ModelState.IsValid)
                {
                    if (rLevelService.Exists(ResourceLevel.ItemName, ResourceLevel.ItemEntryID))
                    {
                        DisplayWarningMessage($"Level Name '{ResourceLevel.ItemName}' is duplicate");
                        return View(ResourceLevel);
                    }

                    ResourceLevelDto ResourceLevelModel = Mapper.Map<ResourceLevelModel, ResourceLevelDto>(ResourceLevel);
                    rLevelService.Update(ResourceLevelModel);
                    DisplaySuccessMessage($"Resource Level '{ResourceLevel.ItemName}' details have been modified successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(ResourceLevel);
        }

        // POST: ResourceLevel/Delete/5
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
                if (rLevelService.CanBeDeleted(id.Value) == false)
                {
                    DisplayWarningMessage("There are some dependencies with this Resource Level. So, you can't delete this for now");
                    return RedirectToAction("List");
                }

                rLevelService.Delete(new ResourceLevelDto { ItemEntryID = id.Value });
                DisplaySuccessMessage("Resource Level has been deleted successfully");
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult GetResourceLevelsByBuID(int levelID)
        {
            IEnumerable<ResourceLevelDto> ResourceLevels = rLevelService.GetAllByLevel(levelID).ToList();

            List<SelectListItem> ResourceLevelItems = (from p in ResourceLevels
                                                       select new SelectListItem
                                                       {
                                                           Value = p.ItemEntryID.ToString(),
                                                           Text = p.ItemName
                                                       }).ToList();
            return Json(ResourceLevelItems);
        }

        private IEnumerable<ResourceLevelModel> GetResourceLevels(int pageNo)
        {
            IEnumerable<ResourceLevelDto> ResourceLevels = rLevelService.GetAll(RecordsPerPage, pageNo);
            IEnumerable<ResourceLevelModel> ResourceLevelModels = Mapper.Map<IEnumerable<ResourceLevelDto>, IEnumerable<ResourceLevelModel>>(ResourceLevels);
            return ResourceLevelModels;
        }

        private void LoadDropDownItems()
        {
            IEnumerable<BuLevelDto> buLevels = buLevelService.GetAll();

            List<SelectListItem> buListItems = (from c in buLevels
                                                orderby c.ItemName
                                                select new SelectListItem
                                                {
                                                    Text = c.ItemName,
                                                    Value = c.ItemEntryID.ToString()
                                                }).ToList();

            ViewBag.BuLevelListItems = buListItems;
        }
    }
}
