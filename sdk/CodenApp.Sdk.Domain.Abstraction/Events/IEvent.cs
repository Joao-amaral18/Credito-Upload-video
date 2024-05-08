using MediatR;
using System;

namespace CodenApp.Sdk.Domain.Abstraction.Events
{
    public interface IEvent : INotification
    {
        DateTime Timestamp { get; }
    }
}
