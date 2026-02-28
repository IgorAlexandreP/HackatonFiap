using HackatonFiap.Compartilhado;

namespace HackatonFiap.Analise.Repositorios;

public class AlertasRepositorio
{
    private readonly List<AlertaDTO> _alertas = new();
    
    public void Adicionar(AlertaDTO alerta)
    {
        _alertas.Add(alerta);
    }
    
    public List<AlertaDTO> ObterTodos()
    {
        return _alertas.ToList();
    }
}
