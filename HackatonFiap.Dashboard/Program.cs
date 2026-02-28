using HackatonFiap.Dashboard.Components;
using HackatonFiap.Dashboard.Providers;
using HackatonFiap.Dashboard.Servicos;
using Microsoft.AspNetCore.Components.Authorization;
using Prometheus;

var construtor = WebApplication.CreateBuilder(args);

// Add services to the container.
construtor.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Autenticação no Blazor Server
construtor.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "Cookies";
    })
    .AddCookie("Cookies");

construtor.Services.AddAuthorization();
construtor.Services.AddCascadingAuthenticationState();
construtor.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
construtor.Services.AddScoped<CustomAuthenticationStateProvider>(provider => 
    (CustomAuthenticationStateProvider)provider.GetRequiredService<AuthenticationStateProvider>());
    
// Scoped Session
construtor.Services.AddScoped<SessaoUsuario>();

// Register services with HttpClient
construtor.Services.AddHttpClient<IdentidadeService>(client => 
    client.BaseAddress = new Uri(construtor.Configuration["Services:Identidade"] ?? "http://localhost:5188"));

construtor.Services.AddHttpClient<PropriedadesService>(client => 
    client.BaseAddress = new Uri(construtor.Configuration["Services:Propriedades"] ?? "http://localhost:5132"));

construtor.Services.AddHttpClient<IngestaoService>(client => 
    client.BaseAddress = new Uri(construtor.Configuration["Services:Ingestao"] ?? "http://localhost:5074"));

construtor.Services.AddHttpClient<AnaliseService>(client => 
    client.BaseAddress = new Uri(construtor.Configuration["Services:Analise"] ?? "http://localhost:5200"));

construtor.Services.AddHttpClient<ClimaService>(client => 
    client.BaseAddress = new Uri("https://api.open-meteo.com/"));

var aplicacao = construtor.Build();

// Configure the HTTP request pipeline.
if (!aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseExceptionHandler("/Error", createScopeForErrors: true);
    aplicacao.UseHsts();
}

aplicacao.UseRouting();
aplicacao.UseHttpMetrics();
aplicacao.MapMetrics();

aplicacao.UseHttpsRedirection();
aplicacao.UseStaticFiles();
aplicacao.UseAntiforgery();
aplicacao.UseAuthentication();
aplicacao.UseAuthorization();

aplicacao.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

aplicacao.Run();
