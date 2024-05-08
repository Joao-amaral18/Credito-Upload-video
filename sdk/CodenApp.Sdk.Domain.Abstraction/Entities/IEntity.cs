using System;
using System.Collections.Generic;
using System.Text;

namespace CodenApp.Sdk.Domain.Abstraction.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; }
    }
}
