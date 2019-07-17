using Agilisium.TalentManager.WebUI.App_Start;
using Agilisium.TalentManager.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Agilisium.TalentManager.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application[UIConstants.CONFIG_ENABLE_PAGINATION] = ConfigurationManager.AppSettings[UIConstants.CONFIG_ENABLE_PAGINATION];
            Application[UIConstants.CONFIG_RECORDS_PER_PAGE] = ConfigurationManager.AppSettings[UIConstants.CONFIG_RECORDS_PER_PAGE];
            Application[UIConstants.CONFIG_EMAIL_TEMPLATES_FOLDER_PATH] = ConfigurationManager.AppSettings[UIConstants.CONFIG_EMAIL_TEMPLATES_FOLDER_PATH];
            Application[UIConstants.CONFIG_ADMIN_USER_NAME] = ConfigurationManager.AppSettings[UIConstants.CONFIG_ADMIN_USER_NAME];
            Application[UIConstants.CONFIG_IGNORABLE_TEXT_IN_USER_NAME] = ConfigurationManager.AppSettings[UIConstants.CONFIG_IGNORABLE_TEXT_IN_USER_NAME];
            Application["LoggedUserName"] = "";
        }
    }
}
