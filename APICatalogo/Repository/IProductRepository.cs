using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters);
    Task<PagedList<Product>> GetProductsPriceFilterAsync (ProductsPriceFilter productsFilterParameters);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
}
