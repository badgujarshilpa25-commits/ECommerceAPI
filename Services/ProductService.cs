using ECommerceAPI.DTOs;
using ECommerceAPI.Model;
using EcommerceAPI.Data;

namespace EcommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        ILogger<ProductService> _logger;
        public ProductService(AppDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all products from the database.");
                 return _context.Products;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<Product> GetById(int id)
        {
            try
            {
                return _context.Products.FirstOrDefault(p => p.Id == id) ?? new Product();
            }
            catch (Exception ex) { 
                _logger.LogError(ex, "An error occurred while fetching product by ID.");
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<ProductResponseDto> Create(CreateProductDto product)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock
                };

                _context.Products.Add(newProduct);

                await _context.SaveChangesAsync();

                var response = new ProductResponseDto
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Price = newProduct.Price,
                    Category = newProduct.Category,
                    ImageUrl = newProduct.ImageUrl,
                    Stock = newProduct.Stock
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new product.");
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<ProductResponseDto> Update(CreateProductDto product)
        {
            try
            {
                var updatedProduct = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    ImageUrl = product.ImageUrl,
                    Stock = product.Stock
                };

                _context.Update(updatedProduct);

                await _context.SaveChangesAsync();

                var response = new ProductResponseDto
                {
                    Id = updatedProduct.Id,
                    Name = updatedProduct.Name,
                    Price = updatedProduct.Price,
                    Category = updatedProduct.Category,
                    ImageUrl = updatedProduct.ImageUrl,
                    Stock = updatedProduct.Stock
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                throw; // Re-throw the exception to be handled by the caller
            }
        }
    }
}