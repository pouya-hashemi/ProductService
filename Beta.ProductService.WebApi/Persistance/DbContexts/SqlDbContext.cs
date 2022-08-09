using Beta.ProductService.WebApi.Interfaces;
using Beta.ProductService.WebApi.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Beta.ProductService.WebApi.Persistance.DbContexts;

public class SqlDbContext:DbContext,ISqlDbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options):base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }
}