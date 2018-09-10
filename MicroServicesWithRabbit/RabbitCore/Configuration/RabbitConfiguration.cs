using Newtonsoft.Json;
using RabbitCore.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace RabbitCore.Configuration
{
    public class RabbitConfiguration
    {
        readonly static ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "Booking_Test_ES" };
        public static IConnection connection;
        public static IModel channel;
        private static readonly Dictionary<string, Type> handlersDictionary = new Dictionary<string, Type>();
        public void Configure(string serviceName)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            this.CreateEndpoints(serviceName);

            this.ScanEvents();

            this.ScanHandlers(serviceName);
        }

        private void CreateEndpoints(string serviceName)
        {
            channel.ExchangeDeclare(serviceName, ExchangeType.Fanout);
            channel.QueueDeclare(queue: serviceName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.QueueBind(queue: serviceName,
              exchange: serviceName,
              routingKey: "");
        }

        private void ScanEvents()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //Load referenced assemblies for the main axembly
            foreach (var assemblyName in assemblies.Where(x => x.EntryPoint != null).First().GetReferencedAssemblies())
            {
                var referencedAssembly = Assembly.Load(assemblyName);
                assemblies.Add(referencedAssembly);
            }


            var eventTypes = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => typeof(IEvent).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (var type in eventTypes)
            {
                var eventName = type.Name;
                channel.ExchangeDeclare(eventName, ExchangeType.Fanout);
            }
        }

        private void ScanHandlers(string serviceName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            foreach (var assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                var referencedAssembly = Assembly.Load(assemblyName);
                assemblies.Add(referencedAssembly);
            }

            var types = assemblies.SelectMany(x => x.GetTypes());


            foreach (var type in types)
            {
                var handlerInterfaceType = type.GetInterfaces().Where(i => i.Name == typeof(IHandleMessages<>).Name).FirstOrDefault();
                if (handlerInterfaceType != null)
                {
                    var messageType = handlerInterfaceType.GetGenericArguments()[0];
                    //TODO: para hacer este bind comprobar que sea un evento, los commands van directos al exchange del routing
                    //crear binding desde el exchange del mensaje hacia el exchange del endpoint
                    if (typeof(IEvent).IsAssignableFrom(messageType))
                    {
                        channel.ExchangeBind(serviceName, messageType.Name, messageType.Name);
                    }
                    
                    handlersDictionary.Add(messageType.Name, type);
                    
                    //configurar handler
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, eventArgs) =>
                    {
                        this.ResolveHandlerAndExecute(eventArgs);

                        channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                    };
                    channel.BasicConsume(queue: serviceName,
                                     autoAck: false,
                                     consumerTag: $"{ Environment.MachineName }({ Dns.GetHostAddresses(Environment.MachineName)[0] }).{messageType.Name}",
                                     consumer: consumer);

                }
            }
        }

        private void ResolveHandlerAndExecute(BasicDeliverEventArgs eventArgs)
        {
            //CHECK HEADERS TO SEE WHICH HANDLER WE SHOULD RESOLVE AND EXECUTE
            var handledMessageNameBytes = (byte[])eventArgs.BasicProperties.Headers[BusConstants.Header.MessageName];
            var handledMessageName = Encoding.UTF8.GetString(handledMessageNameBytes);

            handlersDictionary.TryGetValue(handledMessageName.ToString(), out Type registeredHandlerType);

            var body = eventArgs.Body;
            var message = Encoding.UTF8.GetString(body);

            //instanciar el handler y llamar al método
            ////////////////////////////////////////////////////////////////////
            MethodInfo methodInfo = registeredHandlerType.GetMethod(BusConstants.Handlers.HandlerMethod);
            if (methodInfo != null)
            {
                object result = null;
                ParameterInfo[] parameters = methodInfo.GetParameters();
                object handlerInstance = Activator.CreateInstance(registeredHandlerType, null);



                if (parameters.Length == 0 || parameters.Length > 1)
                {
                    // This works fine
                    // THROW tiene que tener un parametro con el mensaje
                    //result = methodInfo.Invoke(handlerInstance, null);
                }
                else
                {
                    var deserializedMessageToBeHandled = JsonConvert.DeserializeObject(message, parameters[0].ParameterType);
                    object[] parametersArray = new object[] { deserializedMessageToBeHandled };


                    result = methodInfo.Invoke(handlerInstance, parametersArray);
                }
            }
        }
    }
}
