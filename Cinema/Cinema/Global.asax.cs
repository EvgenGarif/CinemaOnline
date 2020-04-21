using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Cinema.Interfaces;
using Cinema.Services;
using LightInject;

namespace Cinema
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new ServiceContainer();
            container.RegisterControllers();
            container.EnableAnnotatedConstructorInjection();
            InitAutomapperProfiles(container);
            container.Register<HttpContextBase>(factory => new HttpContextWrapper(HttpContext.Current), new PerRequestLifeTime());
            container.Register<ITicketsService, JsonTicketsService>(new PerRequestLifeTime());
            container.EnableMvc();
        }


        private static void InitAutomapperProfiles(ServiceContainer container)
        {
            var assembly = Assembly.GetCallingAssembly();
            var definedTypes = assembly.DefinedTypes;
            var profiles = definedTypes.Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();

            void ConfigAction(IMapperConfigurationExpression cfg)
            {
                foreach (var profile in profiles.Select(x => x.AsType()))
                {
                    cfg.AddProfile(profile);
                }
            }

            Mapper.Initialize(ConfigAction);
            var config = (MapperConfiguration)Mapper.Configuration;
            config.AssertConfigurationIsValid();
            container.Register(sp => config.CreateMapper(), new PerRequestLifeTime());
        }
    }
}
