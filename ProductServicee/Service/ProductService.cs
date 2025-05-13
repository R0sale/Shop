using Service.Contracts;
using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Microsoft.EntityFrameworkCore.Metadata;
using Contracts;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Entities.Models;
using Entities.Exceptions;
using System.Runtime.CompilerServices;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(bool trackChanges)
        {
            var products = await _repository.FindAll(trackChanges)
                .OrderBy(p => p.Name)
                .ToListAsync();

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return productsDTO;
        }

        public async Task<ProductDTO> GetProductAsync(Guid id, bool trackChanges)
        {
            var products = FindAndCheckIfExistsProduct(id, trackChanges);

            var productsDTO = _mapper.Map<ProductDTO>(products);

            return productsDTO;
        }

        public async Task<ProductDTO> CreateProduct(ProductForCreationDTO productForCreation)
        {
            var product = _mapper.Map<Product>(productForCreation);

            _repository.CreateProduct(product);
            await _repository.SaveProductAsync();

            var productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        public async Task DeleteProduct(Guid id, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            _repository.DeleteProduct(product);
            await _repository.SaveProductAsync();
        }

        public async Task UpdateProduct(Guid id, ProductForUpdateDTO productForUpd, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            _mapper.Map(productForUpd, product);
            await _repository.SaveProductAsync();
        }

        public async Task<(ProductForUpdateDTO productForUpd, Product productEntity)> GetProductForPatialUpdate(Guid id, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            var productForUpd = _mapper.Map<ProductForUpdateDTO>(product);

            return (productForUpd, product);
        }

        public async Task SaveChangesForPatrialUpdate(ProductForUpdateDTO productForUpd, Product product)
        {
            _mapper.Map(productForUpd, product);
            await _repository.SaveProductAsync();
        }

        private async Task<Product> FindAndCheckIfExistsProduct(Guid id, bool trackChanges)
        {
            var product = await _repository.FindByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

            if (product is null)
                throw new ProductNotFoundException(id);
            
            return product;
        }
    }
}
