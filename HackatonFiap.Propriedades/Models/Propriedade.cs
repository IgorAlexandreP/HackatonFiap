namespace HackatonFiap.Propriedades.Models;

public class Propriedade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public List<Talhao> Talhoes { get; set; } = new();
}
