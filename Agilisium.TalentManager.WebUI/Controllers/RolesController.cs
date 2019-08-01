using Agilisium.TalentManager.WebUI.Helpers;
using Agilisium.TalentManager.WebUI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Controllers
{
    public class RolesController : BaseController
    {
        private readonly ApplicationDbContext context;

        public RolesController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Roles
        public ActionResult List()
        {
            List<IdentityRole> roles = context.Roles.ToList();
            return View(roles);
        }

        public ActionResult Create()
        {
            IdentityRole role = new IdentityRole();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Roles.Add(role);
                    context.SaveChanges();
                    DisplaySuccessMessage("New Use Role has been created successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(role);
        }

        public ActionResult Edit(string id)
        {
            IdentityRole role = new IdentityRole();
            if (string.IsNullOrWhiteSpace(id))
            {
                DisplayWarningMessage("Looks like, the ID is missing in your request");
                return RedirectToAction("List");
            }

            try
            {
                if (!context.Roles.Any(r => r.Id == id))
                {
                    DisplayWarningMessage($"Sorry, We couldn't find a Role with ID: {id}");
                    return RedirectToAction("List");
                }

                role = context.Roles.FirstOrDefault(r => r.Id == id);
            }
            catch (Exception exp)
            {
                DisplayReadErrorMessage(exp);
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityRole oldRole = context.Roles.FirstOrDefault(r => r.Id == role.Id);
                    oldRole.Name = role.Name;
                    //context.Roles.Attach(oldRole);
                    context.SaveChanges();
                    DisplaySuccessMessage("Use Role details has been updated successfully");
                    return RedirectToAction("List");
                }
            }
            catch (Exception exp)
            {
                DisplayUpdateErrorMessage(exp);
            }
            return View(role);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            DisplayWarningMessage("This function is not yet activated");
            return RedirectToAction("List");
            ////try
            ////{
            ////    if (service.IsReservedEntry(id.Value))
            ////    {
            ////        DisplayWarningMessage("Hey, why do you want to delete a Reserved Category. Please check with the system administrator.");
            ////        return RedirectToAction("List");
            ////    }

            ////    if (service.CanBeDeleted(id.Value) == false)
            ////    {
            ////        DisplayWarningMessage("There are some dependencies with this Category. So, you can't delete this for now");
            ////        return RedirectToAction("List");
            ////    }

            ////    service.DeleteCategory(new DropDownCategoryDto { CategoryID = id.Value });
            ////    DisplaySuccessMessage($"Category has been deleted successfully");
            ////}
            ////catch (Exception exp)
            ////{
            ////    DisplayDeleteErrorMessage(exp);
            ////}
            //return RedirectToAction("List");
        }
    }
}