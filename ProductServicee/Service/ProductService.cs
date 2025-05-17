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
using Shared.Request;
using System.Security.Claims;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpClient _client;

        public ProductService(IProductRepository repository, IMapper mapper, IHttpClient client)
        {
            _repository = repository;
            _mapper = mapper;
            _client = client;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductParams productParams, bool trackChanges)
        {
            var products = await _repository.GetAllProducts(productParams, trackChanges);

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return productsDTO;
        }

        public async Task<ProductDTO> GetProductAsync(Guid id, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            var productsDTO = _mapper.Map<ProductDTO>(product);

            return productsDTO;
        }

        public async Task<ProductDTO> CreateProduct(ProductForCreationDTO productForCreation)
        {
            var product = _mapper.Map<Product>(productForCreation);
            var owner = await _client.GetUser(productForCreation.OwnerId);

            if (owner is null)
                throw new UserNotFoundException(productForCreation.OwnerId);

            _repository.CreateProductRep(product);
            await _repository.SaveAsync();

            var productDTO = _mapper.Map<ProductDTO>(product);

            return productDTO;
        }

        public async Task DeleteProduct(Guid id, ClaimsPrincipal User, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            await CheckValidOwner(product, User);


            _repository.DeleteProductRep(product);
            await _repository.SaveAsync();
        }

        public async Task UpdateProduct(Guid id, ProductForUpdateDTO productForUpd, ClaimsPrincipal User, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            await CheckValidOwner(product, User);

            _mapper.Map(productForUpd, product);
            await _repository.SaveAsync();
        }

        public async Task<(ProductForUpdateDTO productForUpd, Product productEntity)> GetProductForPatialUpdate(Guid id, ClaimsPrincipal User, bool trackChanges)
        {
            var product = await FindAndCheckIfExistsProduct(id, trackChanges);

            await CheckValidOwner(product, User);

            var productForUpd = _mapper.Map<ProductForUpdateDTO>(product);

            return (productForUpd, product);
        }

        public async Task SaveChangesForPatrialUpdate(ProductForUpdateDTO productForUpd, Product product)
        {
            _mapper.Map(productForUpd, product);
            await _repository.SaveAsync();
        }

        private async Task<Product> FindAndCheckIfExistsProduct(Guid id, bool trackChanges)
        {
            var product = await _repository.GetProduct(id, trackChanges);

            if (product is null)
                throw new ProductNotFoundException(id);
            
            return product;
        }

        private async Task CheckValidOwner(Product product, ClaimsPrincipal User)
        {
            var owner = await _client.GetUser(product.OwnerId);

            if (owner is null)
                throw new UserNotFoundException(product.OwnerId);

            if (!owner.UserName.Equals(User.Identity.Name))
                throw new UserNotCorrespondException(owner.Id);
        }
    }
}
