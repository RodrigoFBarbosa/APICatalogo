using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) // se eu precisar de uma instancia do contexto, eu busco na classe base (protected em repository)
    {
     
    }

    public async Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParameters)
    {
        var categories = await GetAllAsync();

        var ordenedCategories = categories.OrderBy(c => c.CategoryId).AsQueryable();

        var result = PagedList<Category>.ToPagedList(ordenedCategories, categoriesParameters.PageNumber, categoriesParameters.PageSize);

        return result;
    }

    public async Task<PagedList<Category>> GetCategoriesNameFilterAsync(CategoriesNameFilter categoriesFilterParameters)
    {
        var categories = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriesFilterParameters.Name))
        {
            categories = categories.Where(c => c.Name.Contains(categoriesFilterParameters.Name));
        }

        var filteredCategories = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesFilterParameters.PageNumber, categoriesFilterParameters.PageSize);
        

        return filteredCategories;
    }
}
