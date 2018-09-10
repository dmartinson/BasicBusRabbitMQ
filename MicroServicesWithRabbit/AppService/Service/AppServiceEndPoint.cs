using BookingService.Messages.Commands;
using BookingService.Messages.Constants;
using RabbitCore.Configuration;
using RabbitCore.Messages;
using System;
using System.Threading;

namespace AppService.Service
{
    public class AppServiceEndPoint
    {
        public void Start()
        {
            //Configure Bus
            var rabbitConfiguration = new RabbitConfiguration();
            rabbitConfiguration.Configure("MainClientAppService");
            //Configure where the commands sent by this service are forwarded to.
            Transport.RouteToEndpoint(typeof(MakeAppointment), BookingServiceConstants.ServiceName);

            var bus = new Publisher();
            //User Input
            do
            {
                Console.WriteLine("Enter a date with format (dd/mm/yyyy): ");
                var ddMMyyyy = Convert.ToString(Console.ReadLine());
                var splitedValues = ddMMyyyy.Split('/');
                var makeAppointmentCommand = new MakeAppointment
                {
                    Date = new DateTime(int.Parse(splitedValues[2]), int.Parse(splitedValues[1]), int.Parse(splitedValues[0]))
                };
                Console.WriteLine("What is your Name?");
                makeAppointmentCommand.Name = Convert.ToString(Console.ReadLine());
                bus.Send(makeAppointmentCommand);

                Thread.Sleep(5000);
            } while (true);
        }
        public void Stop()
        {
            // write code here that runs when the Windows Service stops.  
        }
    }
}
