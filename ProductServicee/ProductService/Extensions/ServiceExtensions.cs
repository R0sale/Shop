using Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace ProductService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureProductRepository(this IServiceCollection service)
        {
            service.AddScoped<IProductRepository, ProductRepository>();
        }

        public static void ConfigureServiceManager(this IServiceCollection service)
        {
            service.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureSqlContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ProductRepositoryContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
            });
        }
    }
}
