using APICatalogo.Context;
using APICatalogo.Migrations;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // como eu tenho um um metodo na instancia especifica para product também preciso injetar ele
        // como o repositororio especifico tambem implemente o repository generico, eu nao preciso instancia-lo aqui neste caso
        private readonly IUnitOfWork _uof;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork uof)
        {

            _logger = logger;
            _uof = uof;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<Product>> GetProductsCategories(int id)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound();

            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
            
        {
            var products = _uof.ProductRepository.GetAll();

            if (products is null)
            {
                _logger.LogWarning($"");
                return NotFound(); 
            }

            return Ok(products);
        }

        
        [HttpGet("{id:int}", Name = "GettingProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
            {
                _logger.LogWarning($"Produto com id = {id} não encontrado...");
                return NotFound("Product not found");
            }

            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }
                
           var createdProduct =  _uof.ProductRepository.Create(product);
            _uof.Commit();
            
            return new CreatedAtRouteResult("GettingProduct", 
                new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Product> Put(int id, Product product)
        {
            if (id != product.ProductId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }

            _uof.ProductRepository.Update(product);
            _uof.Commit();
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
           var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
            {
                _logger.LogWarning($"produto com id = {id} não encontrado...");
                return NotFound($"produto com id = {id} não encontrado...");
            }

            var deletedProduct = _uof.ProductRepository.Delete(product);
            _uof.Commit();
            return Ok(deletedProduct);


        }
    }
}
