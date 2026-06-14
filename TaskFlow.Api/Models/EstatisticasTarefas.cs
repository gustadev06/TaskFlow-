namespace TaskFlow.Api.Models;

public class EstatisticasTarefas
{
    public int Total { get; set; }
    public int Concluidas { get; set; }
    public int Pendentes { get; set; }
    public Dictionary<string, int> PorPrioridade { get; set; } = new();
}