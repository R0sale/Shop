using Service.Contracts;
using Contracts;
using AutoMapper;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;

        public ServiceManager(IProductRepository productRepository, IMapper mapper, IHttpClient httpClient)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(productRepository, mapper, httpClient));
        }

        public IProductService ProductService => _productService.Value;
    }
}
