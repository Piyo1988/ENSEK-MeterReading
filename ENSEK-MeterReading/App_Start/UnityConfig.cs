using System.Web.Http;
using Unity;
using Unity.WebApi;
using ENSEK_MeterReading.Models.BL;
using ENSEK_MeterReading.Models.DAL;

namespace ENSEK_MeterReading
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IMeterReadingRepository, MeterReadingRepositorycs>();
            container.RegisterType<IMeterReadingService, MeterReadingService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}