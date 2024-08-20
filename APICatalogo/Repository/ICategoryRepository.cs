using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository;

public interface ICategoryRepository : IRepository<Category>
{
    Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters);
    Task<PagedList<Category>> GetCategoriesNameFilterAsync(CategoriesNameFilter categoriesFilterParameters);
}
