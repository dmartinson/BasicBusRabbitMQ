namespace RabbitCore.Messages
{
    public interface IHandleMessages<T>
    {
        void Handle(T message);
    }
}
