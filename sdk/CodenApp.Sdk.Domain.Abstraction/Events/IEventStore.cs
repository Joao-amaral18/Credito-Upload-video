namespace CodenApp.Sdk.Domain.Abstraction.Events
{
    public interface IEventStore
    {
        void Save<T>(T @event) where T : Event;
    }
}
