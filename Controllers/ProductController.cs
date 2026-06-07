using EcommerceAPI.Services;
using ECommerceAPI.DTOs;
using ECommerceAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        IProductService _productService;
        ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            this._productService = productService;
            this._logger = logger;
        }

        [HttpGet()]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _productService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                    return NotFound();
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product by ID.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
        {
            try
            {
                var newProduct = await _productService.Create(product);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateProduct([FromBody] CreateProductDto product)
        {
            try
            {
                var updatedProduct = await _productService.Update(product);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}