using MediatR;

namespace CodenApp.Sdk.Domain.Abstraction.Events
{
    public abstract class Message : IRequest<bool>
    {
        public short EntityId { get; protected set; }
        public string MessageType { get; protected set; }

        protected Message()
        {
            this.MessageType = GetType().Name;
        }
    }
}
