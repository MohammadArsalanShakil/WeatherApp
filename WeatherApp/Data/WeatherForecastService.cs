using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace WeatherApp.Data
{
    public class WeatherForecastService
    {
        private static string APIKEY = ""; // Add your API Key

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public async Task<WeatherAPI> GetForecastAsync(string CityName)
        {
            if (!string.IsNullOrWhiteSpace(CityName) && !int.TryParse(CityName, out _))
            {
                HttpClient clientCoordinates = new HttpClient() { BaseAddress = new Uri("https://api.openweathermap.org/") };
                var responseCoordinates = await clientCoordinates.GetFromJsonAsync<object>($"/geo/1.0/direct?q={CityName}&limit=1&appid={APIKEY}");
                CityAPI cityAPI = JsonConvert.DeserializeObject<List<CityAPI>>(responseCoordinates.ToString())[0];


                HttpClient client = new HttpClient() { BaseAddress = new Uri("https://api.openweathermap.org/") };
                var response = await client.GetFromJsonAsync<object>($"/data/2.5/weather?lat={cityAPI.lat}&lon={cityAPI.lon}&appid={APIKEY}");
                WeatherAPI weatherAPI = JsonConvert.DeserializeObject<WeatherAPI>(response.ToString());


                return weatherAPI;
            }
            else
            {
                return new WeatherAPI { };
            }
        }
    }
}