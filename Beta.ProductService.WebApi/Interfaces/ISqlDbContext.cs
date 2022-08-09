using Beta.ProductService.WebApi.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beta.ProductService.WebApi.Interfaces;

public interface ISqlDbContext
{
    DbSet<Product> Products { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}