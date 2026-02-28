using HackatonFiap.Compartilhado;
using System.Net.Http.Json;

namespace HackatonFiap.Dashboard.Servicos;

public class IdentidadeService
{
    private readonly HttpClient _http;
    private readonly SessaoUsuario _sessao;

    public IdentidadeService(HttpClient http, SessaoUsuario sessao)
    {
        _http = http;
        _sessao = sessao;
    }

    public async Task<bool> Login(string email, string senha)
    {
        var resposta = await _http.PostAsJsonAsync("api/autenticacao/login", new UsuarioDTO { Email = email, Senha = senha });
        if (resposta.IsSuccessStatusCode)
        {
            var tokenDto = await resposta.Content.ReadFromJsonAsync<TokenDTO>();
            _sessao.Token = tokenDto?.Token;
            return true;
        }
        return false;
    }
}
