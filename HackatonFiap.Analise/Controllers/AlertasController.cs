using HackatonFiap.Analise.Repositorios;
using HackatonFiap.Compartilhado;
using Microsoft.AspNetCore.Mvc;

namespace HackatonFiap.Analise.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertasController : ControllerBase
{
    private readonly AlertasRepositorio _repositorio;

    public AlertasController(AlertasRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public ActionResult<List<AlertaDTO>> ObterTodos()
    {
        var alertas = _repositorio.ObterTodos();
        Console.WriteLine($"[AlertasController] Retornando {alertas.Count} alertas.");
        return Ok(alertas);
    }
}
