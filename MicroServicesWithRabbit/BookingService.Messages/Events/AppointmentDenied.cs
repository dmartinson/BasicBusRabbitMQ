using RabbitCore.Messages;

namespace BookingService.Messages.Events
{
    public class AppointmentDenied : IEvent
    {
        public string Reason { get; set; }
    }
}
