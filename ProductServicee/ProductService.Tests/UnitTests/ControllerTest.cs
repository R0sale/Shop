﻿using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Shared.Request;
using Shared.DTOObjects;
using ProductService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProductService.Tests.UnitTests
{
    public class ControllerTest
    {
        [Fact]
        public async Task GetAllProductsTest()
        {
            bool testTrackChanges = false;
            var testProductParams = new ProductParams
            {
                CurrentPage = 1,
                MaxCreationDate = DateTime.Now,
                PageSize = 5
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(s => s.ProductService.GetAllProductsAsync(testProductParams, testTrackChanges))
                .ReturnsAsync(GetProducts());
            var controller = new ProductController(mockService.Object);

            var result = await controller.GetAllProducts(testProductParams);

            var viewresult = Assert.IsType<OkObjectResult>(result);
            var productsAssert = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(viewresult.Value);

            Assert.Equal(2, productsAssert.Count());
            Assert.Equal(StatusCodes.Status200OK, viewresult.StatusCode);
        }

        [Fact]
        public async Task GetProductTest()
        {
            bool testTrackChanges = false;
            Guid testId = Guid.NewGuid();

            var testProduct = new ProductDTO
            {
                Id = testId,
                Price = 10.2m,
                Name = "jujag",
                CreationDate = DateTime.Now,
                Accessibility = true,
                Description = "dadada",
                OwnerId = Guid.NewGuid()
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(s => s.ProductService.GetProductAsync(testId, testTrackChanges))
                .ReturnsAsync(testProduct);
            var controller = new ProductController(mockService.Object);

            var result = await controller.GetProduct(testId);

            var viewresult = Assert.IsType<OkObjectResult>(result);
            var productsAssert = Assert.IsAssignableFrom<ProductDTO>(viewresult.Value);
            Assert.Equal(testProduct.Id, productsAssert.Id);
            Assert.Equal(testProduct.Name, productsAssert.Name);
            Assert.Equal(testProduct.Price, productsAssert.Price);
            Assert.Equal(testProduct.Description, productsAssert.Description);
            Assert.Equal(testProduct.CreationDate, productsAssert.CreationDate);
            Assert.Equal(testProduct.Accessibility, productsAssert.Accessibility);
            Assert.Equal(testProduct.OwnerId, productsAssert.OwnerId);
            Assert.Equal(StatusCodes.Status200OK, viewresult.StatusCode);
        }

        [Fact]
        public async Task CreateProductTest()
        {
            var now = DateTime.Now;

            var testCreationProduct = new ProductForCreationDTO
            {
                Price = 10.2m,
                Name = "jujag",
                CreationDate = now,
                Accessibility = true,
                Description = "dadada",
                OwnerId = Guid.NewGuid()
            };

            var testProduct = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Price = 10.2m,
                Name = "jujag",
                CreationDate = now,
                Accessibility = true,
                Description = "dadada",
                OwnerId = Guid.NewGuid()
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(s => s.ProductService.CreateProduct(testCreationProduct))
                .ReturnsAsync(testProduct);
            var controller = new ProductController(mockService.Object);

            var result = await controller.CreateProduct(testCreationProduct);

            var viewresult = Assert.IsType<CreatedAtRouteResult>(result);
            var productsAssert = Assert.IsAssignableFrom<ProductDTO>(viewresult.Value);
            Assert.Equal(testProduct.Name, productsAssert.Name);
            Assert.Equal(testProduct.Price, productsAssert.Price);
            Assert.Equal(testProduct.Description, productsAssert.Description);
            Assert.Equal(testProduct.CreationDate, productsAssert.CreationDate);
            Assert.Equal(testProduct.Accessibility, productsAssert.Accessibility);
            Assert.Equal(testProduct.OwnerId, productsAssert.OwnerId);
            Assert.Equal(StatusCodes.Status201Created, viewresult.StatusCode);
        }

        [Fact]
        public async Task DeleteProductTest()
        {
            var now = DateTime.Now;
            var testId = Guid.NewGuid();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user-id"),
                new Claim(ClaimTypes.Role, "User")
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(s => s.ProductService.DeleteProduct(testId, It.IsAny<ClaimsPrincipal>(), false))
                .Returns(Task.CompletedTask);
            var controller = new ProductController(mockService.Object);

            var result = await controller.DeleteProduct(testId);

            var viewresult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, viewresult.StatusCode);
        }

        [Fact]
        public async Task UpdateProductTest()
        {
            var now = DateTime.Now;
            var testId = Guid.NewGuid();

            var testProductForUpd = new ProductForUpdateDTO
            {
                Price = 10.2m,
                Name = "jujag",
                CreationDate = now,
                Accessibility = true,
                Description = "dadada",
                OwnerId = Guid.NewGuid()
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user-id"),
                new Claim(ClaimTypes.Role, "User")
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(s => s.ProductService.UpdateProduct(testId, testProductForUpd, It.IsAny<ClaimsPrincipal>(), false))
                .Returns(Task.CompletedTask);
            var controller = new ProductController(mockService.Object);

            var result = await controller.UpdateProduct(testId, testProductForUpd);

            var viewresult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, viewresult.StatusCode);
        }

        [Fact]
        public async Task PartiallyUpdateProductTest()
        {
            var productId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            var patchDoc = new JsonPatchDocument<ProductForUpdateDTO>();
            patchDoc.Replace(p => p.Description, "Updated Description");

            var productForUpdateDto = new ProductForUpdateDTO
            {
                Description = "Old Description",
                Price = 10.0m,
                Name = "Old Name",
                Accessibility = true
            };

            var productEntity = new Product
            {
                Id = productId,
                Price = 10.2m,
                Name = "jujag",
                CreationDate = DateTime.Now,
                Accessibility = true,
                Description = "dadada",
                OwnerId = Guid.NewGuid()
            };

            var mockService = new Mock<IServiceManager>();
            mockService.Setup(x => x.ProductService.GetProductForPatialUpdate(productId, It.IsAny<ClaimsPrincipal>(), true))
                .ReturnsAsync((productForUpdateDto, productEntity));

            mockService.Setup(x => x.ProductService.SaveChangesForPatrialUpdate(productForUpdateDto, productEntity))
                .Returns(Task.CompletedTask);

            var controller = new ProductController(mockService.Object);

            controller.ObjectValidator = new Mock<IObjectModelValidator>().Object;

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, ownerId.ToString()),
            }, "mock"));

            var result = await controller.PartiallyUpdateProduct(productId, patchDoc);

            Assert.IsType<NoContentResult>(result);
        }

        private IEnumerable<ProductDTO> GetProducts()
        {
            var list = new List<ProductDTO>
            {
                new ProductDTO
                {
                    Id = Guid.NewGuid(),
                    Price = 10.2m,
                    Name = "jujag",
                    CreationDate = DateTime.Now,
                    Accessibility = true,
                    Description = "dadada",
                    OwnerId = Guid.NewGuid()
                },
                new ProductDTO
                {
                    Id = Guid.NewGuid(),
                    Price = 11.2m,
                    Name = "jsfdsag",
                    CreationDate = DateTime.Now,
                    Accessibility = true,
                    Description = "cvzsadsaa",
                    OwnerId = Guid.NewGuid()
                }
            };

            return list;
        }
    }
}
