using AlertService.Service;
using Topshelf;

namespace AlertService.Configuration
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<AlertServiceEndPoint>(service =>
                {
                    service.ConstructUsing(s => new AlertServiceEndPoint());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("AlertServiceService");
                configure.SetDisplayName("AlertServiceService");
                configure.SetDescription("Windows Service for Alerts to Test Rabbit MQ.");
            });
        }
    }
}
