

using SmartTraficControlSystem.Utilities.Models;

namespace SmartTraficControlSystem.Components.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartTraficControlSystemAPI");
        }
        public async Task<bool> LoginAsync(AccountModel account)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", account);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RegisterAsync(AccountModel account)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/register", account);
            return response.IsSuccessStatusCode;
        }
    }
}