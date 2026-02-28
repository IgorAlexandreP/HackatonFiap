using HackatonFiap.Compartilhado;
using System.Net.Http.Json;

namespace HackatonFiap.Dashboard.Servicos;

public class IngestaoService
{
    private readonly HttpClient _http;

    public IngestaoService(HttpClient http)
    {
        _http = http;
    }

    public async Task EnviarLeitura(LeituraSensorDTO leitura)
    {
        await _http.PostAsJsonAsync("api/sensores", leitura);
    }
}
