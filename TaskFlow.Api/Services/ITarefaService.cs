using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

public interface ITarefaService
{
    Task<List<Tarefa>> ListarAsync();
    Task<Tarefa?> ObterAsync(int id);
    Task<Tarefa> CriarAsync(string titulo, string prioridade);
    Task<Tarefa?> AtualizarAsync(int id, string titulo, string prioridade, bool concluida);
    Task<Tarefa?> ConcluirAsync(int id);
    Task<bool> RemoverAsync(int id);
    Task<EstatisticasTarefas> ObterEstatisticasAsync();
}
