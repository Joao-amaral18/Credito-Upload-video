namespace CodenApp.Sdk.Domain.Abstraction.Entities
{
    public abstract class EntityBase<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; protected set; }
    }
}