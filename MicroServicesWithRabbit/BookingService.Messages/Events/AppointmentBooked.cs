using RabbitCore.Messages;
using System;

namespace BookingService.Messages.Events
{
    public class AppointmentBooked : IEvent
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
