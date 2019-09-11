﻿using log4net;
using System;
using System.Text;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.Helpers
{
    [Authorize]
    public class BaseController : Controller
    {
        private const string readErrorMessage = "Oops! an error has occured while retrieving the details";
        private const string updateErrorMessage = "Oops! an error has occured while updating the details";
        private const string deleteErrorMessage = "Oops! an error has occured while deleting the details";
        private const string loadErrorMessage = "Oops! an error has occured while loading the page";

        private readonly ILog logger;

        public BaseController()
        {
            CacheHelper.SetContext(HttpContext);
            logger = log4net.LogManager.GetLogger(typeof(BaseController));
        }

        public virtual void DisplayWarningMessage(string message)
        {
            TempData["WarningMessage"] = message;
        }

        public virtual void DisplaySuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        public virtual void DisplayErrorMessage(string message, Exception exp = null)
        {
            TempData["ErrorMessage"] = message;

            logger.Error(message, exp);
        }

        public virtual void DisplayReadErrorMessage(Exception exp)
        {
            DisplayErrorMessage(readErrorMessage);

            logger.Error(exp);
        }

        public virtual void DisplayUpdateErrorMessage(Exception exp)
        {
            DisplayErrorMessage(updateErrorMessage);

            logger.Error(exp);
        }

        public virtual void DisplayDeleteErrorMessage(Exception exp)
        {
            DisplayErrorMessage(deleteErrorMessage);

            logger.Error(exp);
        }

        public virtual void DisplayLoadErrorMessage(Exception exp)
        {
            DisplayErrorMessage(loadErrorMessage);

            logger.Error(exp);
        }

        public bool IsPaginationEnabled
        {
            get
            {
                object configValue = HttpContext.Application[UIConstants.CONFIG_ENABLE_PAGINATION];
                if (configValue == null)
                {
                    return true;
                }

                return configValue.ToString().ToLower() == "true";
            }
        }

        public string EmailTemplatesFolderPath => HttpContext.Application[UIConstants.CONFIG_EMAIL_TEMPLATES_FOLDER_PATH]?.ToString();

        public string AdminUserName => HttpContext.Application[UIConstants.CONFIG_ADMIN_USER_NAME]?.ToString();

        public string LoggedInUserName
        {
            get
            {
                StringBuilder adminName = new StringBuilder(HttpContext.User.Identity.Name);
                string ignorableString = HttpContext.Application[UIConstants.CONFIG_IGNORABLE_TEXT_IN_USER_NAME]?.ToString();
                if (!string.IsNullOrWhiteSpace(ignorableString))
                {
                    string[] spl = { "__" };
                    string[] ignorableNames = ignorableString.Split(spl, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string name in ignorableNames)
                    {
                        adminName.Replace(name, "");
                    }
                }
                return adminName.ToString();
            }
        }

        public int RecordsPerPage
        {
            get
            {
                if (IsPaginationEnabled)
                {
                    object pageSizeValue = HttpContext.Application[UIConstants.CONFIG_RECORDS_PER_PAGE];

                    if (int.TryParse(pageSizeValue.ToString(), out int pageSize))
                    {
                        return 10;
                    }
                    return pageSize;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}