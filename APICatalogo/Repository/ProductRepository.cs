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

    public PagedList<Product> GetProducts(ProductsParameters productsParameters)
    {
        var products = GetAll().OrderBy(p => p.ProductId).AsQueryable();

        var ordenedProducts = PagedList<Product>.ToPagedList(products, productsParameters.PageNumber, productsParameters.PageSize);

        return ordenedProducts;
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.CategoryId == id);
    }

    public PagedList<Product> GetProductsPriceFilter(ProductsPriceFilter productsFilterParameters)
    {
        var products = GetAll().AsQueryable();

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

        var filteredProducts = PagedList<Product>.ToPagedList(products, productsFilterParameters.PageNumber, productsFilterParameters.PageSize);

        return filteredProducts;
    }
}
