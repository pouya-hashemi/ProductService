using Beta.ProductService.WebApi.Dtos.ProductDtos;
using Beta.ProductService.WebApi.Exceptions;
using Beta.ProductService.WebApi.Interfaces;
using Beta.ProductService.WebApi.Persistance.Entities;
using Beta.ProductService.WebApi.RabbitMessages;
using Microsoft.EntityFrameworkCore;

namespace Beta.ProductService.WebApi.EntityServices;

public class ProductManager:IProductManager
{
    private readonly ISqlDbContext _context;
    private readonly IRabbitMqPublisher<CreateProductMessage> _createProductPublisher;

    public ProductManager(ISqlDbContext context,
        IRabbitMqPublisher<CreateProductMessage> createProductPublisher)
    {
        _context = context;
        this._createProductPublisher = createProductPublisher;
    }
    public async Task<IEnumerable<ProductReadDto>> GetProductsAsync(
        long? productId, 
        string? searchQuery=default, 
        int pageSize=20 ,
        int pageIndex =1)
    {
        
        var query = _context.Products.AsQueryable();

        if (productId!=null)
        {
            query = query.Where(w => w.Id == productId);
        }

        if (!String.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(w => w.Name.Contains(searchQuery));
        }

        var list = await query
            .OrderByDescending(o => o.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(s=>new ProductReadDto()
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync();

        return list;

    }

    public async Task<Product> CreateProductAsync(ProductCreateDto productDto)
    {
        var product = new Product(productDto.Name);

        await CheckProductDuplication(product);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _createProductPublisher.Publish(new CreateProductMessage(product));


        return product;
    }
    public async Task<Product> UpdateProductAsync(ProductUpdateDto productDto)
    {
        var product = await GetProductIfExists(productDto.Id);

        product.SetName(productDto.Name);
        
        await CheckProductDuplication(product);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        
        return product;
    }

    public async Task DeleteProductAsync(long id)
    {
        var product = await GetProductIfExists(id);

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

    }

    private async Task CheckProductDuplication(Product product)
    {
        var productExists = await _context.Products
            .AnyAsync(prod => prod.Name == product.Name && prod.Id != product.Id);

        if (productExists)
        {
            throw new BadRequestException("This product already exists");
        }
    }

    private async Task<Product> GetProductIfExists(long id)
    {
        var product =await _context.Products.FirstOrDefaultAsync(prod => prod.Id == id);

        if (product==null)
        {
            throw new BadRequestException("Product does not exists");
        }

        return product;
    }
}