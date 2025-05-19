using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Shared.DTOObjects;
using Entities.Models;
using System.Net;
using System.Text.Json;
using System.Net.Http.Json;

namespace ProductService.Tests.IntegrationTests
{
    public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsSuccessStatusCode()
        {
            var request = "/api/products";

            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync();

            Assert.Contains("Chair", content.Result);
        }

        [Fact]
        public async Task GetProduct_ReturnsSuccessStatusCode()
        {
            var id = new Guid("3b6e3995-046a-4c51-a65a-a5d419e23783");

            var request = $"/api/products/{id}";

            var response = await _client.GetAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chair", content);
        }

        [Fact]
        public async Task CreateProduct_ReturnsUnauthorizedCode()
        {
            var newProduct = new
            {
                Name = "Test",
                Description = "A wooden table",
                Accessibility = true,
                Price = 200.50m,
                CreationDate = DateTime.UtcNow,
                OwnerId = Guid.NewGuid()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/products", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        //works when UserService is on.
        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            var newProduct = new ProductForCreationDTO
            {
                Name = "TestProduct",
                Description = "A wooden table",
                Accessibility = true,
                Price = 200.50m,
                CreationDate = DateTime.UtcNow,
                OwnerId = new Guid("3b6e3995-056a-4c52-a65a-a5d419e23783")
            };

            var token = JwtTokenGenerator.GenerateJwt();

            var content = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/api/products", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        //works when UserService is on.
        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            var token = JwtTokenGenerator.GenerateJwt();
            var id = "2C13FDB2-83A5-4F1A-1829-08DD95525D58";

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync($"/api/products/{id}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        //works when UserService is on.
        [Fact]
        public async Task UpdateProduct_ReturnsNoContent()
        {
            var updProduct = new ProductForUpdateDTO
            {
                OwnerId = new Guid("3b6e3995-056a-4c52-a65a-a5d419e23783"),
                Name = "Orange",
                Description = "orange",
                Accessibility = true,
                Price = 2m,
                CreationDate = new DateTime(2024, 10, 2)
            };

            var content = new StringContent(
                JsonSerializer.Serialize(updProduct),
                Encoding.UTF8,
                "application/json");

            var token = JwtTokenGenerator.GenerateJwt();
            var id = "2C13FDB2-83A5-4F1A-1829-08DD95525D58";

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PutAsync($"/api/products/{id}", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        //works when UserService is on.
        [Fact]
        public async Task PartiallyUpdateProduct_ReturnsNoContent()
        {
            var token = JwtTokenGenerator.GenerateJwt();
            var id = "2C13FDB2-83A5-4F1A-1829-08DD95525D58";

            var patchDoc = new[]
            {
                new { op = "replace", path = "/name", value = "Updated Product Name" }
            };

            var patchContent = new StringContent(JsonSerializer.Serialize(patchDoc), Encoding.UTF8, "application/json-patch+json");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PatchAsync($"/api/products/{id}", patchContent);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        
    }
}
