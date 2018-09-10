using BookingService.Messages.Commands;
using BookingService.Messages.Events;
using RabbitCore.Messages;
using System;
using System.Linq;

namespace BookingService.Handlers
{
    public class MakeAppointmentHandler : IHandleMessages<MakeAppointment>
    {
        private Publisher bus; 

        public void Handle(MakeAppointment message)
        {
            Console.WriteLine($"I received a make Appointment requet for { message.Name } on { message.Date }");

            this.bus = new Publisher();
            if(message.Date < DateTime.Now)
            {
                Console.Write("Date is in the past.");
                bus.Publish(new AppointmentDenied { Reason = "Date is in the past." });
                return;
            }
            var entryToBeAdded = new Tuple<DateTime, string>(message.Date, message.Name);
            var entryFoundInAgenda = Bookings.Bookings.Agenda.Where(ap => ap.Item1 == entryToBeAdded.Item1 && ap.Item2 == entryToBeAdded.Item2).FirstOrDefault();
            if(entryFoundInAgenda != null)
            {
                Console.Write("This was already booked.");
                bus.Publish(new AppointmentDenied { Reason = "This was already booked for that date and name." });
                return;
            }
            Bookings.Bookings.Agenda.Add(entryToBeAdded);

            
            bus.Publish(new AppointmentBooked { Date = message.Date, Name = message.Name });
        }
    }
}
