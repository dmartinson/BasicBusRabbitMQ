using AppService.Service;
using Topshelf;

namespace AppService.Configuration
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<AppServiceEndPoint>(service =>
                {
                    service.ConstructUsing(s => new AppServiceEndPoint());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("AppServiceService");
                configure.SetDisplayName("AppServiceService");
                configure.SetDescription("Windows Service App to Test Rabbit MQ.");
            });
        }
    }
}
