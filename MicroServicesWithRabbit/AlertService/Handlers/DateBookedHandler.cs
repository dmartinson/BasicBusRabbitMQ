using BookingService.Messages.Events;
using RabbitCore.Messages;
using System;

namespace AlertService.Handlers
{
    public class DateBookedHandler : IHandleMessages<AppointmentBooked>
    {
        public void Handle(AppointmentBooked message)
        {
            Console.WriteLine($"I've also heard that an Appointment was booked for { message.Name } on { message.Date }. I will send a reminder to the user.");
        }
    }
}
