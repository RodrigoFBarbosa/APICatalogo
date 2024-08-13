using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Migrations;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork uof, IMapper mapper)
        {

            _logger = logger;
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("products/{id}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsCategories(int id)
        {
            var products = _uof.ProductRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound();
         // var destino = _mapper.Map<Destino>(origem);
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
            
        {
            var products = _uof.ProductRepository.GetAll();

            if (products is null)
            {
                _logger.LogWarning($"");
                return NotFound(); 
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        
        [HttpGet("{id:int}", Name = "GettingProduct")]
        public ActionResult<ProductDTO> Get(int id)
        {
            var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
            {
                _logger.LogWarning($"Produto com id = {id} não encontrado...");
                return NotFound("Product not found");
            }

            var productDto = _mapper.Map<ProductDTO>(product);

            return productDto;
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post(ProductDTO productDto)
        {
            if (productDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }
             // mudando de DTO para product
            var product = _mapper.Map<Product>(productDto);
                
            var createdProduct =  _uof.ProductRepository.Create(product);
            _uof.Commit();

            var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);
            
            return new CreatedAtRouteResult("GettingProduct", 
                new { id = createdProductDto.ProductId }, createdProductDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> Put(int id, ProductDTO productDto)
        {
            if (id != productDto.ProductId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = _uof.ProductRepository.Update(product);
            _uof.Commit();

            var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
           var product = _uof.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
            {
                _logger.LogWarning($"produto com id = {id} não encontrado...");
                return NotFound($"produto com id = {id} não encontrado...");
            }

            var deletedProduct = _uof.ProductRepository.Delete(product);
            _uof.Commit();

            var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDto);


        }
    }
}
