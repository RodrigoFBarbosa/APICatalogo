﻿using APICatalogo.Context;
using APICatalogo.Migrations;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get() // poderia utilizar List mas o IEnumerable é mais otimizado
            //ActionResult permite retornar ou uma lista de produto ou todos os metodos suportados pelo ActionResult
        {
            var products = _context.Products.ToList();

            if (products is null)
            {
                return NotFound(); //404
            }

            return products;
        }

        // retornar produto por Id
        [HttpGet("{id:int}", Name = "GettingProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
                // se nao encontrar retorna null

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return product;
        }

        [HttpPost]
        public ActionResult Post(Product product)
        {
            if (product is null)
                return BadRequest();
            

            _context.Products.Add(product); // adiciona na memoria
            _context.SaveChanges(); // persiste os dados na tabela
            
            return new CreatedAtRouteResult("GettingProduct", 
                new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            //var product = _context.Products.Find(id);

            if ( product is null)
                return NotFound("Product not found");

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(product);


        }
    }
}
