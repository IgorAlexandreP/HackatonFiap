using HackatonFiap.Analise.Repositorios;
using HackatonFiap.Analise.Servicos;
using Prometheus;

var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddControllers();
construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

construtor.Services.AddSingleton<AlertasRepositorio>();
construtor.Services.AddSingleton<IAnalisadorRisco, AnalisadorRisco>();
construtor.Services.AddHostedService<ConsumidorSensor>();

var aplicacao = construtor.Build();

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseRouting();
aplicacao.UseHttpMetrics();
aplicacao.MapMetrics();

aplicacao.MapControllers();
aplicacao.Run();
