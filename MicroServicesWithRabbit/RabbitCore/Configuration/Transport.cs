using System;
using System.Collections.Generic;

namespace RabbitCore.Configuration
{
    public static class Transport
    {
        public static readonly Dictionary<string, string> RoutingForCommands = new Dictionary<string, string>();
        public static void RouteToEndpoint(Type command, string endpoint)
        {
            RoutingForCommands.Add(command.Name, endpoint);
        }
    }
}
