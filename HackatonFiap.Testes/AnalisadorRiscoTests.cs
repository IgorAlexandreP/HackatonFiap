using HackatonFiap.Analise.Servicos;
using HackatonFiap.Compartilhado;
using Xunit;

namespace HackatonFiap.Testes;

public class AnalisadorRiscoTests
{
    private readonly AnalisadorRisco _analisador;

    public AnalisadorRiscoTests()
    {
        _analisador = new AnalisadorRisco();
    }

    [Fact]
    public void Deve_Gerar_Alerta_Quando_Umidade_Baixa()
    {
        // Arrange
        var leitura = new LeituraSensorDTO { Umidade = 20, Temperatura = 25, TalhaoId = 1 };

        // Act
        bool resultado = _analisador.Analisar(leitura, out var alerta);

        // Assert
        Assert.True(resultado);
        Assert.NotNull(alerta);
        Assert.Equal("Crítico", alerta.Nivel);
        Assert.Contains("Seca Crítica", alerta.Mensagem);
    }

    [Fact]
    public void Nao_Deve_Gerar_Alerta_Para_Condicoes_Normais()
    {
        // Arrange
        var leitura = new LeituraSensorDTO { Umidade = 50, Temperatura = 25, TalhaoId = 1 };

        // Act
        bool resultado = _analisador.Analisar(leitura, out var alerta);

        // Assert
        Assert.False(resultado);
        Assert.Null(alerta);
    }

    [Fact]
    public void Deve_Gerar_Alerta_Quando_Temperatura_Alta()
    {
        // Arrange
        var leitura = new LeituraSensorDTO { Umidade = 40, Temperatura = 45, TalhaoId = 2 };

        // Act
        bool resultado = _analisador.Analisar(leitura, out var alerta);

        // Assert
        Assert.True(resultado);
        Assert.NotNull(alerta);
        Assert.Equal("Alto", alerta.Nivel);
        Assert.Contains("Calor Extremo", alerta.Mensagem);
    }
}
