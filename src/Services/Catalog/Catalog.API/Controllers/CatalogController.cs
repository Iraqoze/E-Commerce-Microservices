using System.Net;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController:ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository repo, ILogger<CatalogController> logger)
        {
            _repository= repo;
            _logger=logger;
        }

        [HttpGet("GetProductsAsync")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync(){
          var products=  await _repository.GetProducts();
          return Ok(products);
        } 

        [HttpGet("GetProductByNameAsync")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByNameAsync(string name){
            var products= await _repository.GetProductByName(name);
            return Ok(products);
        }
        
        [HttpGet("GetProductByCategoryAsync")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryAsync(string category){
            var products= await _repository.GetProductyCategory(category);
            return Ok(products);
        }

        [HttpGet("GetProductByIdAsync")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductByIdAsync(string id){
            var product=await _repository.GetProduct(id);
            if(product is null)
                return NotFound();
            return Ok(product);
        }

        [HttpDelete("DeleteProductAsync")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProductAsync(string id){
            var result= await _repository.DeleteProduct(id);
            if(!result)
                return BadRequest();
            return Ok();
        }

        [HttpPost("CreateProductAsync")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> CreateProductAsync([FromBody]Product product){
            await _repository.CreateProduct(product);
            return CreatedAtAction("GetProductByIdAsync",new{id=product.Id},product);
        }
        
        [HttpPut("UpdateProductAsync")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateProductAsync([FromBody]Product product){

          return Ok(await _repository.UpdateProduct(product));
        }        
    }
    
}