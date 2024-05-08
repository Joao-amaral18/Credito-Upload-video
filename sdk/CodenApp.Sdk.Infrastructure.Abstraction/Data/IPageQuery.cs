using System.Collections.Generic;

namespace CodenApp.Sdk.Infrastructure.Abstraction.Data;

public interface IPageQuery<TEntity> : IPageFilter
{
    IEnumerable<TEntity> Itens { get; set; }
    long Total { get; set; }
    int CurrentPage { get; set; }
    int QuantityPerPage { get; set; }
    string Order { get; set; }
    string Clause { get; set; }
    int TotalPages { get; set; }
    string Next { get; set; }
    string Previous { get; set; }
}
