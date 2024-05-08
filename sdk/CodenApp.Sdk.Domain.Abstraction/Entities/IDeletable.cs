using System;

namespace CodenApp.Sdk.Domain.Abstraction.Entities
{
    public interface IDeletable
    {
        bool IsDeleted { get; }
        DateTime? DeletedDate { get; }
        void Delete();
    }
}
