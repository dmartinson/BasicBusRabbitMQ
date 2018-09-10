using BookingService.Messages.Events;
using RabbitCore.Messages;
using System;
namespace AppService.Handlers
{
    public class AppointmentDeniedHandler : IHandleMessages<AppointmentDenied>
    {
        public void Handle(AppointmentDenied message)
        {
            Console.WriteLine($"Appointment denied because { message.Reason }");
        }
    }
}
