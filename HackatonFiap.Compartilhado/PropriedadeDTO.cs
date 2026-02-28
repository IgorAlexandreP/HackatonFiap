namespace HackatonFiap.Compartilhado;

public class PropriedadeDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public List<TalhaoDTO> Talhoes { get; set; } = new List<TalhaoDTO>();
}
