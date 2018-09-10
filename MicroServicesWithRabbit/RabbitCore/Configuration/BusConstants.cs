namespace RabbitCore.Configuration
{
    public static class BusConstants
    {
        public static class Header {
            public const string MessageName = "messageName";
            public const string JsonContentType = "application/json";
        }

        public static class Handlers
        {
            public const string HandlerMethod = "Handle";
        }
    }
}
