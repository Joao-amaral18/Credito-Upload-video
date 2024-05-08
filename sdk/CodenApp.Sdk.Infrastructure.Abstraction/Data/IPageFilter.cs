namespace CodenApp.Sdk.Infrastructure.Abstraction.Data;

public interface IPageFilter
{
    int CurrentPage { get; set; }
    int QuantityPerPage { get; set; }
}
