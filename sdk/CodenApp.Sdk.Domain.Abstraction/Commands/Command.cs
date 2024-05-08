using CodenApp.Sdk.Domain.Abstraction.Events;
using FluentValidation.Results;
using System;

namespace CodenApp.Sdk.Domain.Abstraction.Commands
{
    public abstract class Command : Message
    {
        public DateTime CreationDate { get; set; }
        public ValidationResult ValidationResult { get; set; }
        protected Command()
        {
            CreationDate = DateTime.Now;
        }

        public abstract bool IsValid();
    }
}