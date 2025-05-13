using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(bool trackChanges);
        Task<ProductDTO> GetProductAsync(Guid id, bool trackChanges);
        Task<ProductDTO> CreateProduct(ProductForCreationDTO productForCreation);
        Task DeleteProduct(Guid id, bool trackChanges);
        Task UpdateProduct(Guid id, ProductForUpdateDTO productForUpd, bool trackChanges);
    }
}
