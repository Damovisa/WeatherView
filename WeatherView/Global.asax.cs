using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using LaunchDarkly.Client;
using WeatherView.Data;
using WeatherView.FeatureFlags;
using Module = Autofac.Module;

namespace WeatherView
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WeatherViewModule());
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public class WeatherViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces();

            builder.RegisterType<WeatherFileCacheProvider>()
                .As<IWeatherCacheProvider>();

            builder.RegisterType<LdClient>().AsSelf().SingleInstance()
                .WithParameter("apiKey", ConfigurationManager.AppSettings["LaunchDarklyApiKey"]);

            builder.RegisterType<LaunchDarklyFeatureFlagProvider>()
                .As<IFeatureFlagProvider>();
        }
    }
}
