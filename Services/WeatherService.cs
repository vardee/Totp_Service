using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace totp_Module.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _city;

        private const string BaseUrl = "http://api.openweathermap.org/data/2.5/forecast";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "46a999f5e443d82f2e0ff6eace54b925";
            _city = "1489425";
        }

        public async Task<string> GetRandomData()
        {
            try
            {
                var url = $"{BaseUrl}?id={_city}&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    throw new Exception("Не удалось получить случайные данные");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при получении случайных данных", ex);
            }
        }
    }
}
