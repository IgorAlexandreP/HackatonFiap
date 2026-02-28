using HackatonFiap.Compartilhado;
using HackatonFiap.Ingestao.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.Ingestao.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensoresController : ControllerBase
{
    private readonly PublicadorMensagem _publicador;

    public SensoresController(PublicadorMensagem publicador)
    {
        _publicador = publicador;
    }

    [HttpPost]
    public async Task<IActionResult> ReceberLeitura([FromBody] LeituraSensorDTO leitura)
    {
        await _publicador.PublicarAsync(leitura, "fila_sensores");
        return Accepted();
    }
}
