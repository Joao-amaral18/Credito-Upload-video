namespace CodenApp.Sdk.Domain.Abstraction.Events
{
    public interface IHandler<T> where T : Message
    {
        void Handle(T message);
    }
}