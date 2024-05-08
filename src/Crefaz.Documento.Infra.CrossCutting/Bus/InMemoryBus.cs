﻿using CodenApp.Sdk.Domain.Abstraction.Commands;
using CodenApp.Sdk.Domain.Abstraction.Events;
using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using MediatR;

namespace Crefaz.Credito.Infra.CrossCutting.Bus;

public class InMemoryBus : IMediatorHandler
{
    private readonly IEventStore eventStore;
    private readonly IMediator mediator;

    public InMemoryBus(IEventStore eventStore, IMediator mediator)
    {
        this.eventStore = eventStore;
        this.mediator = mediator;
    }

    public Task RaiseEventAsync<T>(T @event) where T : Event
    {
        if (!@event.MessageType.Equals("DomainNotification"))
            eventStore?.Save(@event);

        return mediator.Publish(@event);
    }

    public Task SendCommandAsync<T>(T command) where T : Command
    {
        return mediator.Send(command);
    }

    public Task Subscribe<T, TH>()
        where T : Event
        where TH : INotificationHandler<T>
    {
        throw new NotImplementedException();
    }
}