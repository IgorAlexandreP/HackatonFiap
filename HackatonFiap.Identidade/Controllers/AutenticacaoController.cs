using HackatonFiap.Compartilhado;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HackatonFiap.Identidade.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] UsuarioDTO usuario)
    {
        if (usuario.Email == "admin@agro.com" && usuario.Senha == "admin123")
        {
            var manipuladorToken = new JwtSecurityTokenHandler();
            var chave = Encoding.ASCII.GetBytes("SegredoSuperSecretoDoHackatonFiap2026");
            var descritorToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = manipuladorToken.CreateToken(descritorToken);
            return Ok(new TokenDTO 
            { 
                Token = manipuladorToken.WriteToken(token),
                Expiracao = descritorToken.Expires.Value 
            });
        }
        return Unauthorized();
    }
}
