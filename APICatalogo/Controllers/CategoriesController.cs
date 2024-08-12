using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Migrations;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories =  _uof.CategoryRepository.GetAll();
        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GettingCategory")]
    public ActionResult<Category> Get(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada...");
            return NotFound($"Categoria com id = {id} não encontrada...");
        }
        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest($"Dados inválidos...");
        }
            
        var createdCategory = _uof.CategoryRepository.Create(category);
        _uof.Commit(); // savechanges implementado na unit of work

        return new CreatedAtRouteResult("GettingCategory", new { id = createdCategory.CategoryId }, createdCategory);
        //201
    }

    [HttpPut("{id:int}")]
    public ActionResult<Category> Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest($"Dados inválidos...");
        }
            
        _uof.CategoryRepository.Update(category);
        _uof.Commit();
        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _uof.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
        {
            _logger.LogWarning($"Categoria com o {id} não encontrada");
            return NotFound($"Categoria com o {id} não encontrada");
        }

        var deletedCategory = _uof.CategoryRepository.Delete(category);
        _uof.Commit();
        return Ok(deletedCategory);
    }
}
