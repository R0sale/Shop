﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.Request;
using System.Linq.Expressions;

namespace Repository
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(ProductRepositoryContext context) : base(context)
        {
        }

        public async Task<PagedList<Product>> GetAllProducts(ProductParams productParams, bool trackChanges)
        {
            var products = await FindAll(trackChanges)
                .FilterProduct(productParams.MinPrice, productParams.MaxPrice, productParams.MinCreationDate, productParams.MaxCreationDate)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return PagedList<Product>.ToPagedList(products, productParams.CurrentPage, productParams.PageSize);
        }

        public async Task<Product> GetProduct(Guid id, bool trackChanges)
        {
            var product = await FindByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

            return product;
        }

        public void CreateProduct(Product product)
        {
            CreateProduct(product);
        }

        public void DeleteProduct(Product product)
        {
            DeleteProduct(product);
        }
    }
}
