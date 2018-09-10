using BookingService.Service;
using Topshelf;

namespace BookingService.Configuration
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<BookingServiceEndPoint>(service =>
                {
                    service.ConstructUsing(s => new BookingServiceEndPoint());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName("BookingServiceService");
                configure.SetDisplayName("BookingServiceService");
                configure.SetDescription("Windows Service for Bookings to Test Rabbit MQ.");
            });
        }
    }
}
