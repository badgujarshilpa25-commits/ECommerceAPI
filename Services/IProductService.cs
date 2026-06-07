using ECommerceAPI.DTOs;
using ECommerceAPI.Model;

namespace EcommerceAPI.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        Task<ProductResponseDto> Create(CreateProductDto product);
        Task<ProductResponseDto> Update(CreateProductDto product);
        Task Delete(int id);
    }
}