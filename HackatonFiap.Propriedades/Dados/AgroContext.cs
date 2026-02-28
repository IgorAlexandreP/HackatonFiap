using HackatonFiap.Propriedades.Models;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.Propriedades.Dados;

public class AgroContext : DbContext
{
    public AgroContext(DbContextOptions<AgroContext> opcoes) : base(opcoes) { }
    public DbSet<Propriedade> Propriedades { get; set; }
    public DbSet<Talhao> Talhoes { get; set; }
}
