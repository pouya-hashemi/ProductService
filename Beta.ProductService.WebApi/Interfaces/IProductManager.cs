using Beta.ProductService.WebApi.Dtos.ProductDtos;
using Beta.ProductService.WebApi.Persistance.Entities;

namespace Beta.ProductService.WebApi.Interfaces;

public interface IProductManager
{
    Task<IEnumerable<ProductReadDto>> GetProductsAsync(
        long? productId,
        string? searchQuery=default,
        int pageSize=20,
        int pageIndex=1);

    Task<ProductReadDto> CreateProductAsync(ProductCreateDto productDto);
    Task<ProductReadDto> UpdateProductAsync(ProductUpdateDto productDto);
    Task DeleteProductAsync(long id);
}