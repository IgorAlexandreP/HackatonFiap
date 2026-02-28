using HackatonFiap.Compartilhado;
using System.Net.Http.Json;

namespace HackatonFiap.Dashboard.Servicos;

public class AnaliseService
{
    private readonly HttpClient _http;

    public AnaliseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AlertaDTO>> ObterAlertas()
    {
        try
        {
            var response = await _http.GetAsync("api/alertas");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[AnaliseService] Erro ao buscar alertas. Status: {response.StatusCode}");
                return new List<AlertaDTO>();
            }
            return await response.Content.ReadFromJsonAsync<List<AlertaDTO>>() ?? new List<AlertaDTO>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AnaliseService] Exceção ao buscar alertas: {ex.Message}");
            return new List<AlertaDTO>();
        }
    }
}
