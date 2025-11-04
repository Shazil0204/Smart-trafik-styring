
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SmartTraficControlSystem.Utilities.Models;

namespace SmartTraficControlSystem.Components.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartTrafficControlSystemAPI");
        }

        public async Task<bool> LoginAsync(AccountModel account)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account/login", account);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                // Network error
                return false;
            }
            catch (TaskCanceledException)
            {
                // Timeout
                return false;
            }
        }

        public async Task<bool> RegisterAsync(AccountModel account)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user/create", account);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                // Network error
                return false;
            }
            catch (TaskCanceledException)
            {
                // Timeout
                return false;
            }
        }
    }
}