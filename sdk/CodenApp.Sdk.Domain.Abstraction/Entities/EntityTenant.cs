namespace CodenApp.Sdk.Domain.Abstraction.Entities
{
    public abstract class EntityTenant<TPrimaryKey> : EntityBase<TPrimaryKey>
    {
        public int tenantId { get; protected set; }

    }
}
