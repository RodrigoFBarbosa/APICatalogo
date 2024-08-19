namespace APICatalogo.Pagination;

public class ProductsPriceFilter : QueryStringParameters
{
    public decimal? Price { get; set; }
    public string? PriceCriteria { get; set; } // maior, menor ou igual
}
