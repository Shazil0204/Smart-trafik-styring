using SmartTraficControlSystem.Utilities.Models;

namespace SmartTraficControlSystem.Components.Services
{
    public class SensorService
    {
        private readonly HttpClient _httpClient;

        public SensorService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SmartTrafficControlSystemAPI");
        }




        public async Task<List<SensorDataModel>> GetSensorDataAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<SensorDataModel>>("api/sensorlog");
                return response ?? new List<SensorDataModel>();
            }
            catch (HttpRequestException)
            {
                // Handle network errors
                return new List<SensorDataModel>();
            }
            catch (TaskCanceledException)
            {
                // Handle timeout
                return new List<SensorDataModel>();
            }
        }
    }
}