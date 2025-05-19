using Client.Extensions;
using Contracts;
using Entities.Exceptions;
using Shared.DTOObjects;
using System.Net.Http.Json;

namespace Client
{
    public class ProductClient : IHttpClient
    {
        private readonly HttpClient _client;

        public ProductClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserDTO> GetUser(Guid id)
        {
            var response = await _client.GetAsync($"https://localhost:7269/api/users/{id}");

            var user = await response.Content.ReadFromJsonAsync<UserDTO>();

            return user;
        }
    }
}
