using System.Security.Claims;
using HackatonFiap.Dashboard.Servicos;
using Microsoft.AspNetCore.Components.Authorization;

namespace HackatonFiap.Dashboard.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly SessaoUsuario _sessao;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(SessaoUsuario sessao)
    {
        _sessao = sessao;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_sessao.Token))
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(_sessao.Token), "jwt"));
            return Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(_anonymous));
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
