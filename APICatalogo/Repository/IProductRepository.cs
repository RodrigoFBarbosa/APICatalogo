using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProducts(ProductsParameters productsParameters);
    PagedList<Product> GetProductsPriceFilter (ProductsPriceFilter productsFilterParameters);
    IEnumerable<Product> GetProductsByCategory(int id);
}
