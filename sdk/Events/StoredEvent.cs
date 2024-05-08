using CodenApp.Sdk.Domain.Abstraction.Events;
using System;

namespace CodenApp.Sdk.Infrastructure.Events
{
    public class StoredEvent : Event
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string User { get; set; }

        public StoredEvent()
        {

        }

        public StoredEvent(Event @event, string data, string user)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            EntityId = @event.EntityId;
            MessageType = @event.MessageType;
            Data = data;
            User = user;
        }
    }
}
