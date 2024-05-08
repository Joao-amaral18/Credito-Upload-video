using System;
using CodenApp.Sdk.Domain.Abstraction.Commands;
using CodenApp.Sdk.Domain.Notifications;
using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CodenApp.Sdk.Domain.CommandHandlers
{
    public class CommandHandler
    {
        private readonly IMediatorHandler mediator;
        private readonly DomainNotificationHandler domainNotifications;
        private readonly ILogger logger;

        public CommandHandler(
            IMediatorHandler mediator,
            ILogger<CommandHandler> logger,
            INotificationHandler<DomainNotification> domainNotifications)
        {
            this.mediator = mediator;
            this.domainNotifications = (DomainNotificationHandler)domainNotifications;
            this.logger = logger;
        }

        protected void NotifyValidationErrors(Command command)
        {
            foreach (var erro in command.ValidationResult?.Errors)
            {
                AddNotification(command.MessageType, erro.ErrorMessage);
            }
        }

        protected async void AddNotification(string key, string error)
        {
            await mediator.RaiseEventAsync(new DomainNotification(key, error));
        }

        protected bool IsValid()
        {
            return !domainNotifications.HasNotification();
        }

    }
}
