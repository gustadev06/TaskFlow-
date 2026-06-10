namespace TaskFlow.Api.Models;

// Entidade persistida no banco (tabela "Tarefas")
public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Concluida { get; set; }
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
}
