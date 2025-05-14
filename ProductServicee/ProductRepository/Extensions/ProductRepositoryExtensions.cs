using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class ProductRepositoryExtensions
    {
        public static IQueryable<Product> FilterProduct(this IQueryable<Product> products, decimal minPrice, decimal maxPrice, DateTime minCreationDate, DateTime maxCreationDate)
        {
            var filteredProduct = products.Where(p => p.Price < maxPrice && 
            p.Price > minPrice && 
            p.CreationDate < maxCreationDate && 
            p.CreationDate > minCreationDate);

            return filteredProduct;
        }
    }
}
