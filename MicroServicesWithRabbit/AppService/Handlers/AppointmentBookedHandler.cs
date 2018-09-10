using BookingService.Messages.Events;
using RabbitCore.Messages;
using System;

namespace AppService.Handlers
{
    public class AppointmentBookedHandler : IHandleMessages<AppointmentBooked>
    {
        public void Handle(AppointmentBooked message)
        {
            Console.WriteLine($"Appointment booked for { message.Name } on { message.Date }. Congratulations!!!");
        }
    }
}
