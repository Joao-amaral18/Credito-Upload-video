using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Shared.Extensions
{
    public static class Instance
    {
        private static Guid Id = Guid.NewGuid();
        public static Guid GetInstanceId()
        {
            return Id;
        }        
    }
}