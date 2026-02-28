using HackatonFiap.Compartilhado;

namespace HackatonFiap.Analise.Servicos;

public interface IAnalisadorRisco
{
    bool Analisar(LeituraSensorDTO leitura, out AlertaDTO? alerta);
}

public class AnalisadorRisco : IAnalisadorRisco
{
    public bool Analisar(LeituraSensorDTO leitura, out AlertaDTO? alerta)
    {
        alerta = null;

        // Regra de Negócio: Umidade abaixo de 30% é crítica
        if (leitura.Umidade < 30)
        {
            alerta = new AlertaDTO
            {
                TalhaoId = leitura.TalhaoId,
                Mensagem = $"Alerta de Seca Crítica! Umidade em {leitura.Umidade}%",
                Nivel = "Crítico",
                DataAlerta = DateTime.UtcNow
            };
            return true;
        }

        // Regra Adicional: Temperatura acima de 40 graus
        if (leitura.Temperatura > 40)
        {
             alerta = new AlertaDTO
            {
                TalhaoId = leitura.TalhaoId,
                Mensagem = $"Alerta de Calor Extremo! Temperatura em {leitura.Temperatura}°C",
                Nivel = "Alto",
                DataAlerta = DateTime.UtcNow
            };
            return true;
        }

        return false;
    }
}
