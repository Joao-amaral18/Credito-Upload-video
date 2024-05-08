using CodenApp.Sdk.Domain.Abstraction.Entities;

namespace CodenApp.Sdk.Domain.Entities
{
    public class Tenant : EntityBase<int>
    {

        public string Name { get; private set; }
        public string HostName { get; private set; }
    }
}
