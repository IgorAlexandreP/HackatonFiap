using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace HackatonFiap.Dashboard.Servicos;

public class ClimaService
{
    private readonly HttpClient _http;

    public ClimaService(HttpClient http)
    {
        _http = http;
    }

    public async Task<PrevisaoClima?> ObterPrevisao(double latitude, double longitude)
    {
        try
        {
            // Open-Meteo API (Free)
            // Example: https://api.open-meteo.com/v1/forecast?latitude=-23.55&longitude=-46.63&current_weather=true
            return await _http.GetFromJsonAsync<PrevisaoClima>($"v1/forecast?latitude={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&longitude={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}&current_weather=true");
        }
        catch
        {
            return null;
        }
    }
}

public class PrevisaoClima
{
    [JsonPropertyName("current_weather")]
    public ClimaAtual? CurrentWeather { get; set; }
}

public class ClimaAtual
{
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    [JsonPropertyName("windspeed")]
    public double WindSpeed { get; set; }
    
    [JsonPropertyName("weathercode")]
    public int WeatherCode { get; set; }
}
