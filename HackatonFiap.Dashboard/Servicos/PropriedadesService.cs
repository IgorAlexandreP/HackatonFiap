using HackatonFiap.Compartilhado;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HackatonFiap.Dashboard.Servicos;

public class PropriedadesService
{
    private readonly HttpClient _http;
    private readonly SessaoUsuario _sessao;

    public PropriedadesService(HttpClient http, SessaoUsuario sessao)
    {
        _http = http;
        _sessao = sessao;
    }

    private void ConfigurarToken()
    {
        if (!string.IsNullOrEmpty(_sessao.Token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _sessao.Token);
        }
    }

    public async Task<List<PropriedadeDTO>> ObterTodas()
    {
        ConfigurarToken();
        try
        {
            return await _http.GetFromJsonAsync<List<PropriedadeDTO>>("api/propriedades") ?? new List<PropriedadeDTO>();
        }
        catch
        {
            return new List<PropriedadeDTO>();
        }
    }

    public async Task Criar(PropriedadeDTO dto)
    {
        ConfigurarToken();
        await _http.PostAsJsonAsync("api/propriedades", dto);
    }

    public async Task AdicionarTalhao(int propriedadeId, TalhaoDTO dto)
    {
        ConfigurarToken();
        await _http.PostAsJsonAsync($"api/propriedades/{propriedadeId}/talhoes", dto);
    }
}
