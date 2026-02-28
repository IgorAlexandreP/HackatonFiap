# Hackaton AgroSolutions - 8NETT

Este projeto é a nossa solução de MVP para a plataforma de agricultura de precisão. Desenvolvemos o sistema pensando em escalabilidade e facilidade de manutenção, usando uma arquitetura de microsserviços.

Abaixo, detalho como foi atendido a cada um dos requisitos da entrega.

---

## 1. Desenho da Solução MVP

Nossa arquitetura foi desenhada para separar bem as responsabilidades. Usamos Microsserviços em .NET 9, onde cada parte do sistema cuida de uma função específica:

*   **Identidade:** Cuida apenas do login e segurança (tokens JWT).
*   **Propriedades:** Gerencia o cadastro das fazendas e talhões.
*   **Ingestão:** É a porta de entrada dos dados dos sensores. Ele recebe os dados e joga numa fila (RabbitMQ) para não travar o sistema se chegar muita coisa de uma vez.
*   **Análise:** Pega os dados da fila e verifica se precisa gerar alerta (ex: seca ou calor excessivo).
*   **Dashboard:** É a tela que o produtor vê, feita em Blazor Server para ser rápida e interativa.

### Por que fizemos assim? (Justificativa Técnica)
Escolhemos microsserviços e mensageria (RabbitMQ) principalmente para aguentar o tranco se o número de sensores crescer muito. Se o serviço de análise ficar lento, a fila segura as pontas e nenhum dado é perdido. Além disso, se um serviço cair, o resto continua funcionando.

Para o banco de dados deste MVP, usamos memória (In-Memory) para facilitar os testes e a execução em qualquer máquina, mas o código usa Entity Framework, então mudar para um banco real (SQL) é só trocar uma configuração.

---

## 2. Demonstração da Infraestrutura

Preparamos tudo para rodar em Kubernetes, simulando um ambiente de produção real. Também incluímos monitoramento com Grafana e Prometheus para ver se os serviços estão saudáveis.

### Como rodar a infraestrutura (Passo a Passo)

Se você tiver o Docker Desktop com Kubernetes habilitado, é bem simples:

1.  **Criar as Imagens:** Primeiro, construa as imagens dos serviços rodando estes comandos no terminal, na pasta raiz do projeto:

    ```bash
    docker build -t hackatonfiap/identidade:latest -f HackatonFiap.Identidade/Dockerfile .
    docker build -t hackatonfiap/propriedades:latest -f HackatonFiap.Propriedades/Dockerfile .
    docker build -t hackatonfiap/ingestao:latest -f HackatonFiap.Ingestao/Dockerfile .
    docker build -t hackatonfiap/analise:latest -f HackatonFiap.Analise/Dockerfile .
    docker build -t hackatonfiap/dashboard:latest -f HackatonFiap.Dashboard/Dockerfile .
    ```

2.  **Subir tudo:** Depois, aplique as configurações do Kubernetes que estão na pasta `k8s`:

    ```bash
    kubectl apply -f k8s/rabbitmq.yaml
    kubectl apply -f k8s/observability.yaml
    kubectl apply -f k8s/apps.yaml
    ```

3.  **Acessar:**
    *   O Dashboard estará em: `http://localhost:8080` (Login: admin@agro.com / admin123)
    *   O Grafana (métricas) estará em: `http://localhost:3000` (use `kubectl port-forward service/grafana 3000:3000`)

---

## 3. Demonstração da Esteira de CI/CD

Configuramos um pipeline automático no GitHub Actions. Toda vez que alguém envia código para a branch `main`, o sistema automaticamente:
1.  Baixa o código.
2.  Compila tudo para garantir que não tem erro de sintaxe.
3.  Roda os testes unitários.

### Testes Unitários
Criamos testes automatizados para garantir que as regras de negócio mais importantes não quebrem. Por exemplo, temos testes que garantem que o sistema gera um alerta de "Seca Crítica" se a umidade for menor que 30%.

Para rodar os testes na sua máquina e conferir, use o comando:
```bash
dotnet test
```

---

## 4. Demonstração do MVP (Funcionalidades)

O sistema está funcional e você pode testar os seguintes fluxos:

1.  **Autenticação:** Fazer login como produtor rural.
2.  **Cadastro:** Cadastrar propriedades e talhões.
3.  **Envio de Dados:** Simular o envio de dados de sensores via API (Swagger do serviço de Ingestão).
4.  **Processamento:** Ver o sistema processando esses dados em segundo plano.
5.  **Dashboard:** Ver os alertas aparecendo na tela e os gráficos atualizando.

---

## Como Rodar Localmente (Sem Kubernetes)

Se preferir rodar sem Docker/Kubernetes, você precisa apenas do .NET 9 e do RabbitMQ rodando.

1.  Suba o RabbitMQ no Docker:
    ```bash
    docker run -d --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```

2.  Abra terminais separados e rode cada projeto:
    ```bash
    dotnet run --project HackatonFiap.Identidade
    dotnet run --project HackatonFiap.Propriedades
    dotnet run --project HackatonFiap.Ingestao
    dotnet run --project HackatonFiap.Analise
    dotnet run --project HackatonFiap.Dashboard
    ```

3.  Acesse o sistema pelo link que aparecer no terminal do Dashboard (geralmente https://localhost:7095).
