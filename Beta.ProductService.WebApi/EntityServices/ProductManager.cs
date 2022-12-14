using Beta.ProductService.WebApi.Dtos.ProductDtos;
using Beta.ProductService.WebApi.Exceptions;
using Beta.ProductService.WebApi.Interfaces;
using Beta.ProductService.WebApi.Persistance.Entities;
using Beta.ProductService.WebApi.RabbitMessages;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Beta.ProductService.WebApi.EntityServices;

public class ProductManager:IProductManager
{
    private readonly ISqlDbContext _context;
    private readonly IRabbitMqPublisher _rabbitPublisher;


    public ProductManager(ISqlDbContext context,
        IRabbitMqPublisher rabbhitPublisher
        )
    {
        _context = context;
        this._rabbitPublisher = rabbhitPublisher;
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

    public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto productDto)
    {
        var product = new Product(productDto.Name);

        await CheckProductDuplication(product);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var productReadDto=  product.Adapt<ProductReadDto>();

        _rabbitPublisher.Publish(new CreateProductMessage(productReadDto));


        return productReadDto;
    }
    public async Task<ProductReadDto> UpdateProductAsync(ProductUpdateDto productDto)
    {
        var product = await GetProductIfExists(productDto.Id);

        product.SetName(productDto.Name);
        
        await CheckProductDuplication(product);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        var productReadDto = product.Adapt<ProductReadDto>();

        _rabbitPublisher.Publish(new UpdateProductMessage(productReadDto));

        return productReadDto;
    }

    public async Task DeleteProductAsync(long id)
    {
        var product = await GetProductIfExists(id);

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        _rabbitPublisher.Publish(new DeleteProductMessage(id));

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