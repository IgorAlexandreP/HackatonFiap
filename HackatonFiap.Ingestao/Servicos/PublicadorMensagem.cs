using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HackatonFiap.Ingestao.Servicos;

public class PublicadorMensagem
{
    private readonly IConnectionFactory _fabrica;

    public PublicadorMensagem(IConfiguration configuracao)
    {
        _fabrica = new ConnectionFactory 
        { 
            HostName = configuracao["RabbitMQ:HostName"] ?? "localhost",
            UserName = configuracao["RabbitMQ:UserName"] ?? "guest",
            Password = configuracao["RabbitMQ:Password"] ?? "guest"
        };
    }

    public async Task PublicarAsync<T>(T mensagem, string fila)
    {
        var tentativas = 0;
        const int maxTentativas = 3;

        while (tentativas < maxTentativas)
        {
            try
            {
                using var conexao = await _fabrica.CreateConnectionAsync();
                using var canal = await conexao.CreateChannelAsync();

                await canal.QueueDeclareAsync(queue: fila, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var json = JsonSerializer.Serialize(mensagem);
                var corpo = Encoding.UTF8.GetBytes(json);

                await canal.BasicPublishAsync(exchange: string.Empty, routingKey: fila, body: corpo);
                
                Console.WriteLine($"[PublicadorMensagem] Mensagem enviada para {fila}: {json}");
                return; // Sucesso
            }
            catch (Exception ex)
            {
                tentativas++;
                Console.WriteLine($"[PublicadorMensagem] Falha ao publicar (Tentativa {tentativas}/{maxTentativas}): {ex.Message}");
                if (tentativas >= maxTentativas) throw; // Relança exceção se exceder tentativas
                await Task.Delay(1000); // Espera 1s
            }
        }
    }
}
