using HackatonFiap.Analise.Repositorios;
using HackatonFiap.Compartilhado;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HackatonFiap.Analise.Servicos;

public class ConsumidorSensor : BackgroundService
{
    private readonly AlertasRepositorio _repositorio;
    private readonly IAnalisadorRisco _analisador;
    private readonly IConnectionFactory _fabrica;

    public ConsumidorSensor(AlertasRepositorio repositorio, IAnalisadorRisco analisador, IConfiguration configuracao)
    {
        _repositorio = repositorio;
        _analisador = analisador;
        _fabrica = new ConnectionFactory 
        { 
            HostName = configuracao["RabbitMQ:HostName"] ?? "localhost",
            UserName = configuracao["RabbitMQ:UserName"] ?? "guest",
            Password = configuracao["RabbitMQ:Password"] ?? "guest"
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tentativas = 0;
        IConnection? conexao = null;
        IChannel? canal = null;

        while (!stoppingToken.IsCancellationRequested && conexao == null)
        {
            try
            {
                Console.WriteLine($"[ConsumidorSensor] Tentando conectar ao RabbitMQ...");
                conexao = await _fabrica.CreateConnectionAsync(stoppingToken);
                canal = await conexao.CreateChannelAsync(cancellationToken: stoppingToken);
                
                Console.WriteLine("[ConsumidorSensor] Conectado com sucesso!");

                await canal.QueueDeclareAsync(queue: "fila_sensores", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumidor = new AsyncEventingBasicConsumer(canal);
                consumidor.ReceivedAsync += async (model, ea) =>
                {
                    try 
                    {
                        var corpo = ea.Body.ToArray();
                        var mensagem = Encoding.UTF8.GetString(corpo);
                        
                        Console.WriteLine($"[ConsumidorSensor] Mensagem recebida: {mensagem}");

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var leitura = JsonSerializer.Deserialize<LeituraSensorDTO>(mensagem, options);

                        if (leitura != null)
                        {
                            ProcessarLeitura(leitura);
                        }
                        else
                        {
                            Console.WriteLine("[ConsumidorSensor] Falha ao deserializar mensagem (null).");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ConsumidorSensor] Erro ao processar mensagem: {ex.Message}");
                    }
                    
                    await Task.CompletedTask;
                };

                await canal.BasicConsumeAsync("fila_sensores", autoAck: true, consumer: consumidor);
                
                // Mantém o serviço rodando
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                tentativas++;
                Console.WriteLine($"[ConsumidorSensor] Falha na conexão (Tentativa {tentativas}): {ex.Message}");
                // Espera 5 segundos antes de tentar novamente
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private void ProcessarLeitura(LeituraSensorDTO leitura)
    {
        if (_analisador.Analisar(leitura, out var alerta) && alerta != null)
        {
            _repositorio.Adicionar(alerta);
            Console.WriteLine($"[ALERTA GERADO] Talhão {leitura.TalhaoId} - {alerta.Mensagem}");
        }
        else
        {
            Console.WriteLine($"[LEITURA OK] Talhão {leitura.TalhaoId} - Umidade {leitura.Umidade}% - Temp {leitura.Temperatura}C");
        }
    }
}
