using Core.Common.Injection;
using Core.Common.Reflect;
using System.Windows;

namespace Core.Common.Wpf.Export
{
    public static class RootInjectorExtensions
    {
        public static IInjectionService InitializeRootInjector(this Application application, IInjectionService injector = null)
        {
            if(injector == null)
            {
                injector = new InjectionService();
                injector.RegisterService<ReflectionService>();
                injector.RegisterService(application);

            }

            application.Resources.Add(nameof(RootInjector), injector);
            return injector;
        }
        public static IInjectionService GetRootInjector(this Application application)
        {
            return application.FindResource(nameof(RootInjector)) as IInjectionService;
        }

        public static IInjectionService RootInjector(this Application application)
        {
            return application.GetRootInjector() ?? application.InitializeRootInjector();
        }

    }
}