namespace HackatonFiap.Compartilhado;

public class AlertaDTO
{
    public int TalhaoId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty; // Normal, Alerta, Risco
    public DateTime DataAlerta { get; set; }
}
