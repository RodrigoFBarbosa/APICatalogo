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
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        var categories =  _uof.CategoryRepository.GetAll();

        if (categories is null)
            return NotFound("Não existem categorias...");

       var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories = _uof.CategoryRepository.GetCategories(categoriesParameters);

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
    public ActionResult<CategoryDTO> Get(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada...");
            return NotFound($"Categoria com id = {id} não encontrada...");
        }

        var categoryDto = category.ToCategoryDto();

        return Ok(categoryDto);
    }

    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest($"Dados inválidos...");
        }

        var category = categoryDto.ToCategory();
            
        var createdCategory = _uof.CategoryRepository.Create(category);
        _uof.Commit(); // savechanges implementado na unit of work

        var createdCategoryDto = createdCategory.ToCategoryDto();

        return new CreatedAtRouteResult("GettingCategory", new { id = createdCategoryDto.CategoryId }, createdCategoryDto);
        //201
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest($"Dados inválidos...");
        }

        var category = categoryDto.ToCategory();

        var updatedCategory = _uof.CategoryRepository.Update(category);
        _uof.Commit();

        var updatedCategoryDto = updatedCategory.ToCategoryDto();

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com o {id} não encontrada");
            return NotFound($"Categoria com o {id} não encontrada");
        }

        var deletedCategory = _uof.CategoryRepository.Delete(category);
        _uof.Commit();

        var deletedCategoryDto = deletedCategory.ToCategoryDto();

        return Ok(deletedCategoryDto);
    }
}
