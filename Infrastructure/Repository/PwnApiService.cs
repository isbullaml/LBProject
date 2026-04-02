using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Core.Entities;
using Core.Interfaces;
using Core.Models;

namespace Infrastructure.Services
{
    public class PwnApiService : IPwnApiService
    {
        private readonly HttpClient _httpClient;

        public PwnApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var baseUrl = configuration["PwnApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "LBProject-API");
        }

        public async Task<IEnumerable<PwnBreach>> GetBreachesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<PwnBreach>>("breaches")
                   ?? Enumerable.Empty<PwnBreach>();
        }

        public async Task<PwnBreach?> GetBreachByNameAsync(string name)
        {
            return await _httpClient.GetFromJsonAsync<PwnBreach>($"breach/{name}");
        }
    }
}