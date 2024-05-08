using CodenApp.Sdk.Infrastructure.Abstraction.Data;
using System.Collections.Generic;

namespace CodenApp.Sdk.Infrastructure.Data
{
    public class PageQuery<TEntity> : IPageQuery<TEntity>
    {
        public IEnumerable<TEntity> Itens { get; set; }
        public long Total { get; set; }
        public int CurrentPage { get; set; }
        public int QuantityPerPage { get; set; }
        public string Order { get; set; }
        public string Clause { get; set; }
        public int TotalPages { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
    }
}
