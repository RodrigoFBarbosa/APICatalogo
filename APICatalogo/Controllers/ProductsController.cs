using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Migrations;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsCategories(int id)
        {
            var products = await _uof.ProductRepository.GetProductsByCategoryAsync(id);
            if (products is null)
                return NotFound();
         // var destino = _mapper.Map<Destino>(origem);
            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _uof.ProductRepository.GetProductsAsync(productsParameters);

            return GetProducts(products);
        }

        [HttpGet("filter/price/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsPriceFilter([FromQuery] ProductsPriceFilter productsFilterParameters)
        {
            var products = await _uof.ProductRepository.GetProductsPriceFilterAsync(productsFilterParameters);
            return GetProducts (products);
        }

        private ActionResult<IEnumerable<ProductDTO>> GetProducts(PagedList<Product> products)
        {
            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var produtosDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(produtosDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
            
        {
            var products = await _uof.ProductRepository.GetAllAsync();

            if (products is null)
            {
                _logger.LogWarning($"");
                return NotFound(); 
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        
        [HttpGet("{id:int}", Name = "GettingProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _uof.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product is null)
            {
                _logger.LogWarning($"Produto com id = {id} não encontrado...");
                return NotFound("Product not found");
            }

            var productDto = _mapper.Map<ProductDTO>(product);

            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDto)
        {
            if (productDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }
             // mudando de DTO para product
            var product = _mapper.Map<Product>(productDto);
                
            var createdProduct =  _uof.ProductRepository.Create(product);
            await _uof.CommitAsync();

            var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);
            
            return new CreatedAtRouteResult("GettingProduct", 
                new { id = createdProductDto.ProductId }, createdProductDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if(patchProductDTO is null || id <= 0)
                return BadRequest();

            var product = await _uof.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product is null)
                return NotFound();

            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDTO.ApplyTo(productUpdateRequest, ModelState);
            //aplyto que irá aplicar as alterações parciais no patch

            if(!ModelState.IsValid || TryValidateModel(productUpdateRequest)) 
                return BadRequest(ModelState);

            _mapper.Map(productUpdateRequest, product);
            
            _uof.ProductRepository.Update(product);
            await _uof.CommitAsync();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDto)
        {
            if (id != productDto.ProductId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = _uof.ProductRepository.Update(product);
            await _uof.CommitAsync();

            var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
           var product = await _uof.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product is null)
            {
                _logger.LogWarning($"produto com id = {id} não encontrado...");
                return NotFound($"produto com id = {id} não encontrado...");
            }

            var deletedProduct = _uof.ProductRepository.Delete(product);
            await _uof.CommitAsync();

            var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDto);


        }
    }
}
