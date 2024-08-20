using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{

    public ProductRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<PagedList<Product>> GetProductsAsync(ProductsParameters productsParameters)
    {
        var products = await GetAllAsync();

        var ordenedProducts = products.OrderBy(p => p.ProductId).AsQueryable();

        var result = PagedList<Product>.ToPagedList(ordenedProducts, productsParameters.PageNumber, productsParameters.PageSize);

        return result;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
    {
        var products = await GetAllAsync();

        var productsCategory = products.Where(c => c.CategoryId == id);

        return productsCategory;
    }

    public async Task<PagedList<Product>> GetProductsPriceFilterAsync(ProductsPriceFilter productsFilterParameters)
    {
        var products = await GetAllAsync();

        if(productsFilterParameters.Price.HasValue && !string.IsNullOrEmpty(productsFilterParameters.PriceCriteria))
        {
            if (productsFilterParameters.PriceCriteria.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(p => p.Price > productsFilterParameters.Price.Value).OrderBy(p => p.Price);
            }
            else if (productsFilterParameters.PriceCriteria.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(p => p.Price < productsFilterParameters.Price.Value).OrderBy(p => p.Price);
            }
            else if (productsFilterParameters.PriceCriteria.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(p => p.Price == productsFilterParameters.Price.Value).OrderBy(p => p.Price);
            }
        }

        var filteredProducts = PagedList<Product>.ToPagedList(products.AsQueryable(), productsFilterParameters.PageNumber, productsFilterParameters.PageSize);

        return filteredProducts;
    }
}
