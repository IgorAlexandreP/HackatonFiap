using HackatonFiap.Compartilhado;
using HackatonFiap.Propriedades.Dados;
using HackatonFiap.Propriedades.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.Propriedades.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropriedadesController : ControllerBase
{
    private readonly AgroContext _contexto;

    public PropriedadesController(AgroContext contexto)
    {
        _contexto = contexto;
    }

    [HttpGet]
    public async Task<ActionResult<List<PropriedadeDTO>>> ObterTodas()
    {
        var propriedades = await _contexto.Propriedades
            .Include(p => p.Talhoes)
            .ToListAsync();
            
        var dtos = propriedades.Select(p => new PropriedadeDTO
        {
            Id = p.Id,
            Nome = p.Nome,
            Endereco = p.Endereco,
            Talhoes = p.Talhoes.Select(t => new TalhaoDTO
            {
                Id = t.Id,
                Cultura = t.Cultura,
                Area = t.Area
            }).ToList()
        }).ToList();
        
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult> Criar([FromBody] PropriedadeDTO dto)
    {
        var propriedade = new Propriedade
        {
            Nome = dto.Nome,
            Endereco = dto.Endereco,
            Talhoes = dto.Talhoes.Select(t => new Talhao
            {
                Cultura = t.Cultura,
                Area = t.Area
            }).ToList()
        };
        
        _contexto.Propriedades.Add(propriedade);
        await _contexto.SaveChangesAsync();
        
        return CreatedAtAction(nameof(ObterTodas), new { id = propriedade.Id }, propriedade);
    }

    [HttpPost("{id}/talhoes")]
    public async Task<ActionResult> AdicionarTalhao(int id, [FromBody] TalhaoDTO dto)
    {
        var propriedade = await _contexto.Propriedades.FindAsync(id);
        if (propriedade == null) return NotFound();
        
        var talhao = new Talhao
        {
            Cultura = dto.Cultura,
            Area = dto.Area,
            PropriedadeId = id
        };
        
        _contexto.Talhoes.Add(talhao);
        await _contexto.SaveChangesAsync();
        
        return Ok();
    }
}
