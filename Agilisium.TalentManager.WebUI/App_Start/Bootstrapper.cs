using Agilisium.TalentManager.Repository.Repositories;
using Agilisium.TalentManager.Service.Concreate;
using Agilisium.TalentManager.WebUI.Models;
using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Agilisium.TalentManager.WebUI.App_Start
{
    public class Bootstrapper
    {
        public static void Run(IAppBuilder app)
        {
            SetAutofacContainer(app);
            AutoMapperConfiguration.Configure();
        }

        private static void SetAutofacContainer(IAppBuilder app)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof(ApplicationDbContext).Assembly);
            builder.RegisterAssemblyTypes(typeof(AuthenticationTicket).Assembly);

            // Repositories
            builder.RegisterAssemblyTypes(typeof(DropDownCategoryRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(DropDownCategoryService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            RegisterIdentifyRelatedType(app, builder);

            //builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }

        private static  void RegisterIdentifyRelatedType(IAppBuilder app, ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
        }
    }
}