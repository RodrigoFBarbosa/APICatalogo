using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mapping;
using APICatalogo.Filters;
using APICatalogo.Migrations;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger, IUnitOfWork uof)
    {
        _logger = logger;
        _uof = uof;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categories =  await _uof.CategoryRepository.GetAllAsync();

        if (categories is null)
            return NotFound("Não existem categorias...");

       var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories = await _uof.CategoryRepository.GetCategoriesAsync(categoriesParameters);

        return GetCategories(categories);
    }

    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesNameFilter([FromQuery] CategoriesNameFilter categoriesNameFilter)
    {
        var filteredCategories = await _uof.CategoryRepository.GetCategoriesNameFilterAsync(categoriesNameFilter);

        return GetCategories(filteredCategories);
    }

    private ActionResult<IEnumerable<CategoryDTO>> GetCategories(PagedList<Category> categories)
    {
        var metadata = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GettingCategory")]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada...");
            return NotFound($"Categoria com id = {id} não encontrada...");
        }

        var categoryDto = category.ToCategoryDto();

        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest($"Dados inválidos...");
        }

        var category = categoryDto.ToCategory();
            
        var createdCategory = _uof.CategoryRepository.Create(category);
        await _uof.CommitAsync(); // savechanges implementado na unit of work

        var createdCategoryDto = createdCategory.ToCategoryDto();

        return new CreatedAtRouteResult("GettingCategory", new { id = createdCategoryDto.CategoryId }, createdCategoryDto);
        //201
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest($"Dados inválidos...");
        }

        var category = categoryDto.ToCategory();

        var updatedCategory = _uof.CategoryRepository.Update(category);
        await _uof.CommitAsync();

        var updatedCategoryDto = updatedCategory.ToCategoryDto();

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var category = await _uof.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com o {id} não encontrada");
            return NotFound($"Categoria com o {id} não encontrada");
        }

        var deletedCategory = _uof.CategoryRepository.Delete(category);
        await _uof.CommitAsync();

        var deletedCategoryDto = deletedCategory.ToCategoryDto();

        return Ok(deletedCategoryDto);
    }
}
