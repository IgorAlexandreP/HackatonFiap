using HackatonFiap.Propriedades.Dados;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Prometheus;

var construtor = WebApplication.CreateBuilder(args);

construtor.Services.AddControllers();
construtor.Services.AddEndpointsApiExplorer();
construtor.Services.AddSwaggerGen();

construtor.Services.AddDbContext<AgroContext>(opcoes => opcoes.UseInMemoryDatabase("AgroDb"));

var chaveJwt = "SegredoSuperSecretoDoHackatonFiap2026";
var chaveBytes = Encoding.ASCII.GetBytes(chaveJwt);

construtor.Services.AddAuthentication(opcoes =>
{
    opcoes.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opcoes.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opcoes =>
{
    opcoes.RequireHttpsMetadata = false;
    opcoes.SaveToken = true;
    opcoes.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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
aplicacao.UseAuthentication();
aplicacao.UseAuthorization();
aplicacao.MapControllers();
aplicacao.Run();
