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
    public class CertsController : BaseController
    {
        private readonly ICertificationService certService;
        private readonly IDropDownSubCategoryService subCategoryService;

        public CertsController(ICertificationService certService, IDropDownSubCategoryService subCategoryService)
        {
            this.certService = certService;
            this.subCategoryService = subCategoryService;
        }

        // GET: Certs
        public ActionResult List(string taID, int page = 1)
        {
            CertificationViewModel viewModel = new CertificationViewModel();

            try
            {
                viewModel.TAListItems = (IEnumerable<SelectListItem>)Session["TAListItems"] ?? GetTAListItems();
                if (viewModel.TAListItems.Count() == 0)
                {
                    DisplayWarningMessage("There are no Technology Area has been configured yet. You will not be able to create new Certifications");
                    return View(viewModel);
                }

                if (string.IsNullOrEmpty(taID))
                {
                    if (Session["TAID"] == null
                        || (Session["TAID"] != null && string.IsNullOrEmpty(Session["TAID"].ToString())))
                    {
                        viewModel.SelectedCertID = int.Parse(viewModel.TAListItems.FirstOrDefault(c => c.Text != "Please Select")?.Value);
                    }
                    else
                    {
                        viewModel.SelectedCertID = int.Parse(Session["TAID"].ToString());
                    }
                }
                else
                {
                    viewModel.SelectedCertID = int.Parse(taID);
                }

                viewModel.PagingInfo = new PagingInfo
                {
                    TotalRecordsCount = certService.TotalRecordsCountByTechAreaID(viewModel.SelectedCertID),
                    RecordsPerPage = RecordsPerPage,
                    CurentPageNo = page
                };

                if (viewModel.PagingInfo.TotalRecordsCount > 0)
                {
                    Session["TAID"] = viewModel.SelectedCertID.ToString();
                    viewModel.Certifications = GetCertificates(viewModel.SelectedCertID, page);
                }
                else
                {
                    string certName = certService.GetByID(viewModel.SelectedCertID)?.Name;
                    if (string.IsNullOrEmpty(certName))
                    {
                        DisplayWarningMessage("Hey, please check whether you are trying to access the correct Certitification.");
                    }
                    else
                    {
                        DisplayWarningMessage($"There are no Certifications found for the Technology Area '{certName}'");
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(viewModel);
        }

        // GET: Certs/Create
        public ActionResult Create()
        {
            try
            {
                InitializePageData();
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }

            return View(new CertificationModel());
        }

        // POST: Certs/Create
        [HttpPost]
        public ActionResult Create(CertificationModel cert)
        {
            try
            {
                InitializePageData();

                if (ModelState.IsValid)
                {
                    if (certService.Exists(cert.Name))
                    {
                        DisplayWarningMessage($"The Certification Name '{cert.Name}' is duplicate");
                        return View(cert);
                    }
                    CertificationDto certModel = Mapper.Map<CertificationModel, CertificationDto>(cert);
                    certService.AddCertificate(certModel);
                    DisplaySuccessMessage($"New Certification '{cert.Name}' has been stored successfully");
                    return RedirectToAction("List", new { taID = Session["TAID"] });
                }
            }
            catch (Exception exp)
            {
                DisplayLoadErrorMessage(exp);
            }
            return View(cert);
        }

        // GET: Certs/Edit/5
        public ActionResult Edit(int? id)
        {
            CertificationModel cert = new CertificationModel();

            try
            {
                InitializePageData();

                if (!id.HasValue)
                {
                    DisplayWarningMessage("Looks like, the ID is missing in your request");
                    return RedirectToAction("List", new { taID = Session["TAID"] });
                }

                if (!certService.Exists(id.Value))
                {
                    DisplayWarningMessage($"Sorry, We couldn't find the Certification with ID: {id.Value}");
                    return RedirectToAction("List", new { taID = Session["TAID"] });
                }

                CertificationDto practiceDto = certService.GetByID(id.Value);
                cert = Mapper.Map<CertificationDto, CertificationModel>(practiceDto);
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }

            return View(cert);
        }

        // POST: Certs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CertificationModel cert)
        {
            try
            {
                InitializePageData();

                if (ModelState.IsValid)
                {
                    if (certService.Exists(cert.Name, cert.CertificationID))
                    {
                        DisplayWarningMessage($"Certification Name '{cert.Name}' is duplicate");
                        return View(cert);
                    }

                    CertificationDto certModel = Mapper.Map<CertificationModel, CertificationDto>(cert);
                    certService.Update(certModel);
                    DisplaySuccessMessage($"Certification '{cert.Name}' details have been modified successfully");
                    return RedirectToAction("List", new { taID = Session["TAID"] });
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(cert);
        }

        // POST: Certs/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                DisplayWarningMessage("Looks like, the ID is missing in your request");
                return RedirectToAction("List", new { taID = Session["TAID"] });
            }

            try
            {
                if (certService.CanBeDeleted(id.Value) == false)
                {
                    DisplayWarningMessage("This can't be deleted as there are some dependencies with Employee data");
                    return RedirectToAction("List", new { taID = Session["TAID"] });
                }

                certService.Delete(new CertificationDto { CertificationID = id.Value });
                DisplaySuccessMessage("Certification details has been deleted successfully");
            }
            catch (Exception exp)
            {
                DisplayDeleteErrorMessage(exp);
            }
            return RedirectToAction("List", new { taID = Session["TAID"] });
        }

        //[HttpPost]
        //public JsonResult GetCertificatesByTAID(int areaID)
        //{
        //    IEnumerable<CertificationDto> certs = certService.GetAllByTechnologyArea(areaID);

        //    List<SelectListItem> practiceItems = (from p in certs
        //                                          select new SelectListItem
        //                                          {
        //                                              Value = p.PracticeID.ToString(),
        //                                              Text = p.PracticeName
        //                                          }).ToList();
        //    return Json(practiceItems);
        //}
        private IEnumerable<CertificationModel> GetCertificates(int taID, int pageNo)
        {
            IEnumerable<CertificationDto> certs = certService.GetAllByTechnologyArea(taID, RecordsPerPage, pageNo);
            IEnumerable<CertificationModel> practiceModels = Mapper.Map<IEnumerable<CertificationDto>, IEnumerable<CertificationModel>>(certs);
            return practiceModels;
        }

        private void InitializePageData()
        {
            GetTAListItems();
        }

        private List<SelectListItem> GetTAListItems()
        {
            IEnumerable<DropDownSubCategoryDto> buList = subCategoryService.GetSubCategories((int)CategoryType.TechnologyArea);

            List<SelectListItem> taListItems = (from c in buList
                                                orderby c.SubCategoryName
                                                where c.CategoryID == (int)CategoryType.TechnologyArea
                                                select new SelectListItem
                                                {
                                                    Text = c.SubCategoryName,
                                                    Value = c.SubCategoryID.ToString()
                                                }).ToList();

            ViewBag.TechAreaListItems = taListItems;
            return taListItems;
        }
    }
}
