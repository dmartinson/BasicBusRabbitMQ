using System;

namespace BookingService.Messages.Commands
{
    public class MakeAppointment
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
