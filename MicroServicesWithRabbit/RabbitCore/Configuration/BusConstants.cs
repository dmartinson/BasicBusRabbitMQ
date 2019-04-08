namespace RabbitCore.Configuration
{
    public static class BusConstants
    {
        public static class Header {
            public const string MessageName = "x-message-name";
            public const string RetryCount = "x-retry-count";
            public const string JsonContentType = "application/json";
        }

        public static class Handlers
        {
            public const string HandlerMethod = "Handle";
        }
    }
}
