using Agilisium.TalentManager.WebUI.Helpers;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

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

        protected void Application_PostAuthenticateRequest()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    string[] roles = authTicket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
        }
    }
}