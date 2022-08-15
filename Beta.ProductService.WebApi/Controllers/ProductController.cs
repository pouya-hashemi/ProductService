using Beta.ProductService.WebApi.Dtos.ProductDtos;
using Beta.ProductService.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Beta.ProductService.WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ProductController:ControllerBase
{
    private readonly IProductManager _productManager;

    public ProductController(IProductManager productManager)
    {
        _productManager = productManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts(long? producId,string? searchQuery,int? pageSize,int? pageIndex)
    {
        var products = await _productManager.GetProductsAsync(producId, searchQuery, pageSize??20, pageIndex??1);
        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReadDto>> GetProductById(long? id)
    {
        var products = await _productManager.GetProductsAsync(id);
        return Ok(products.FirstOrDefault());
    }
    
    
    [HttpPost]
    public async Task<ActionResult<long>> CreateProduct([FromBody]ProductCreateDto productDto)
    {
        var productReadDto = await _productManager.CreateProductAsync(productDto);
        return CreatedAtAction("GetProductById",new { productReadDto.Id},productReadDto);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody]ProductUpdateDto productDto)
    {
        await _productManager.UpdateProductAsync(productDto);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        await _productManager.DeleteProductAsync(id);
        return Ok();
    }
}