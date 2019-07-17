using Agilisium.TalentManager.WebUI.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Agilisium.TalentManager.WebUI.Startup))]
namespace Agilisium.TalentManager.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Bootstrapper.Run(app);
        }
    }
}
