using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.Services;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly CosmosProductRepository _repository;

        public ProductsController(CosmosProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id}/{category}")]
        public async Task<IActionResult> Get(string id, string category)
        {
            var product = await _repository.GetAsync(id, category);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var created = await _repository.CreateAsnyc(product);
            return CreatedAtAction(nameof(Get), new {id  = created.Id, category = created.Category}, created);
        }

        [HttpPut("{id}/{category}")]
        public async Task<IActionResult> Update(string id, string category, Product product)
        {
            product.Id = id;
            product.Category = category;

            var updated = await _repository.UpdateAsnyc(product);
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id, string category)
        {
            await _repository.DeleteAsync(id, category);

            return NoContent();
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var products = await _repository.GetByCategoryAsync(category);
            return Ok(products);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _repository.GetPagedAsync(pageNumber, pageSize);
            return Ok(products);
        }

        [HttpPost("batch/{category}")]
        public async Task<IActionResult> BatchInsert(string category, [FromBody]IEnumerable<Product> products)
        {
            var inserted = await _repository.BatchInsertAsync(category, products);

            return Ok(new { inserted });
        }
    }
}
