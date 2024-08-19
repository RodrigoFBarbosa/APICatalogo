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

    public PagedList<Category> GetCategories(CategoriesParameters categoriesParameters)
    {
        var categories = GetAll().OrderBy(c => c.CategoryId).AsQueryable();

        var ordenedCategories = PagedList<Category>.ToPagedList(categories, categoriesParameters.PageNumber, categoriesParameters.PageSize);

        return ordenedCategories;
    }

    public PagedList<Category> GetCategoriesNameFilter(CategoriesNameFilter categoriesFilterParameters)
    {
        var categories = GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(categoriesFilterParameters.Name))
        {
            categories = categories.Where(c => c.Name.Contains(categoriesFilterParameters.Name));
        }

        var filteredCategories = PagedList<Category>.ToPagedList(categories, categoriesFilterParameters.PageNumber, categoriesFilterParameters.PageSize);

        return filteredCategories;
    }
}
