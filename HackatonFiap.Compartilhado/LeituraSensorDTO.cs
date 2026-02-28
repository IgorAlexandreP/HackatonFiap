namespace HackatonFiap.Compartilhado;

public class LeituraSensorDTO
{
    public int TalhaoId { get; set; }
    public double Umidade { get; set; }
    public double Temperatura { get; set; }
    public double Precipitacao { get; set; }
    public DateTime DataLeitura { get; set; }
}
