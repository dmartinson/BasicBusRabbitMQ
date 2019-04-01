using Newtonsoft.Json;
using RabbitCore.Configuration;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace RabbitCore.Messages
{
    public class Publisher
    {
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "Booking_Test_Burgos" };
        public void Publish<T>(T message)
        {
            using (var connection = this.factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var jsonBody = JsonConvert.SerializeObject(message);

                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = BusConstants.Header.JsonContentType;
                    //props.DeliveryMode = 2;
                    props.Headers = new Dictionary<string, object>
                    {
                        { BusConstants.Header.MessageName, message.GetType().Name }
                    };

                    var body = Encoding.UTF8.GetBytes(jsonBody);
                    channel.BasicPublish(exchange: message.GetType().Name,
                                         routingKey: "",
                                         basicProperties: props,
                                         body: body);
                }
            }
        }

        public void Send<T>(T command)
        {
            Transport.RoutingForCommands.TryGetValue(command.GetType().Name, out var endpointName);
            if(endpointName == null)
            {
                throw new KeyNotFoundException($"Missing routing to endpoint configuration for this command { command.GetType().Name }");
            }
            using (var connection = this.factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var jsonBody = JsonConvert.SerializeObject(command);

                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = BusConstants.Header.JsonContentType;
                    //props.DeliveryMode = 2;
                    props.Headers = new Dictionary<string, object>
                    {
                        { BusConstants.Header.MessageName, command.GetType().Name },
                        { BusConstants.Header.RetryCount, 0 }
                    };

                    var body = Encoding.UTF8.GetBytes(jsonBody);
                    channel.BasicPublish(exchange: endpointName,
                                         routingKey: "",
                                         basicProperties: props,
                                         body: body);
                }
            }
        }
    }
}
