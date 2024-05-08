using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodenApp.Sdk.Domain.Abstraction.Events
{
    public abstract class Event : Message, INotification
    {

    }
}
