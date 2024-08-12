using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{

    public ProductRepository(AppDbContext context) : base(context)
    {
        
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.CategoryId == id);
    }
}
