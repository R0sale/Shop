using Entities.Models;
using Shared.Request;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllProducts(ProductParams productParams, bool trackChanges);
        Task<Product> GetProduct(Guid id, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
        Task SaveAsync();
    }
}
