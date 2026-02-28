using HackatonFiap.Ingestao.Servicos;
using Prometheus;

var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddControllers();
construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

construtor.Services.AddSingleton<PublicadorMensagem>();

var aplicacao = construtor.Build();

if (aplicacao.Environment.IsDevelopment())
{
    aplicacao.UseSwagger();
    aplicacao.UseSwaggerUI();
}

aplicacao.UseRouting();
aplicacao.UseHttpMetrics();
aplicacao.MapMetrics();

aplicacao.UseHttpsRedirection();
aplicacao.UseAuthorization();
aplicacao.MapControllers();
aplicacao.Run();
