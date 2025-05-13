using Entities.Models;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IProductRepository
    {
        IQueryable<Product> FindAll(bool trackChanges);
        IQueryable<Product> FindByCondition(Expression<Func<Product, bool>> expression, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        Task SaveProductAsync();
    }
}
