using BookingService.Messages.Constants;
using BookingService.Messages.Events;
using RabbitCore.Configuration;
using RabbitCore.Messages;
using System.Threading;

namespace BookingService.Service
{
    public class BookingServiceEndPoint
    {
        public void Start()
        {
            // write code here that runs when the Windows Service starts up.  

            var rabbitConfiguration = new RabbitConfiguration();
            rabbitConfiguration.Configure(BookingServiceConstants.ServiceName);

            //var publisher = new Publisher();
            //publisher.PublishMessage(new AppointmentBooked { Name = "Diego Test Appointment" });
            //Thread.Sleep(5000);
            //publisher.PublishMessage(new AppointmentDenied { Reason = "Esta ocupado" });
        }
        public void Stop()
        {
            // write code here that runs when the Windows Service stops.  
        }
    }
}
