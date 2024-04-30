using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace totp_Module.Services
{
    public class RandomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _ref;

        private const string BaseUrl = "https://randomapi.com/api";

        public RandomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = "4VR7-WFIL-FN2L-PPQ5";
            _ref = "k8qdfdcx";
        }

        public async Task<string> GetRandomData()
        {
            try
            {
                var url = $"{BaseUrl}/?key={_apiKey}&ref={_ref}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = ParseRandomData(content);
                    return result;
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

        private string ParseRandomData(string jsonData)
        {
            try
            {
                using var doc = JsonDocument.Parse(jsonData);
                var root = doc.RootElement;
                var results = root.GetProperty("results");
                var age = results[0].GetProperty("age").GetString();
                return age;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при разборе JSON-ответа", ex);
            }
        }
    }
}
