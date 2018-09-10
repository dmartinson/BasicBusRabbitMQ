using RabbitCore.Configuration;
using System;

namespace AlertService.Service
{
    public class AlertServiceEndPoint
    {
        public void Start()
        {
            // write code here that runs when the Windows Service starts up.  
            var rabbitConfiguration = new RabbitConfiguration();
            rabbitConfiguration.Configure("AlertService");

            //do
            //{
            //    Console.WriteLine("Enter a date with format (dd/mm/yyyy): ");
            //    var ddMMyyyy = Convert.ToString(Console.ReadLine());
            //    var splitedValues = ddMMyyyy.Split('/');
            //    var reservationDate = new DateTime(int.Parse(splitedValues[2]), int.Parse(splitedValues[1]), int.Parse(splitedValues[0]));
            //} while (true);
        }
        public void Stop()
        {
            // write code here that runs when the Windows Service stops.  
        }
    }
}
