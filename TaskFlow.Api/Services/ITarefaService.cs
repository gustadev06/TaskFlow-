using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

public interface ITarefaService
{
    Task<List<Tarefa>> ListarAsync();
    Task<Tarefa?> ObterAsync(int id);
    Task<Tarefa> CriarAsync(string titulo);
    Task<Tarefa?> AtualizarAsync(int id, string titulo, bool concluida);
    Task<Tarefa?> ConcluirAsync(int id);
    Task<bool> RemoverAsync(int id);
}
