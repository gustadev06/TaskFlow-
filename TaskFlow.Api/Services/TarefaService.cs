using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

// Regras de negocio das tarefas. Toda a persistencia passa por aqui (testavel).
public class TarefaService : ITarefaService
{
    private readonly AppDbContext _db;

    public TarefaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Tarefa>> ListarAsync()
    {
        return await _db.Tarefas
            .OrderBy(t => t.Concluida)
            .ThenByDescending(t => t.CriadaEm)
            .ToListAsync();
    }

    public async Task<Tarefa?> ObterAsync(int id)
    {
        return await _db.Tarefas.FindAsync(id);
    }

    public async Task<Tarefa> CriarAsync(string titulo, string prioridade)
{
    if (string.IsNullOrWhiteSpace(titulo))
        throw new ArgumentException("A tarefa nao pode ser vazia.");

    var tarefa = new Tarefa
    {
        Titulo = titulo.Trim(),
        Prioridade = string.IsNullOrWhiteSpace(prioridade) ? "Média" : prioridade, // Mapeia o campo
        Concluida = false,
        CriadaEm = DateTime.UtcNow
    };

    _db.Tarefas.Add(tarefa);
    await _db.SaveChangesAsync();
    return tarefa;
}

public async Task<Tarefa?> AtualizarAsync(int id, string titulo, string prioridade, bool concluida)
{
    var tarefa = await _db.Tarefas.FindAsync(id);
    if (tarefa is null) return null;

    if (!string.IsNullOrWhiteSpace(titulo))
        tarefa.Titulo = titulo.Trim();

    if (!string.IsNullOrWhiteSpace(prioridade))
        tarefa.Prioridade = prioridade; // Atualiza o campo

    tarefa.Concluida = concluida;
    await _db.SaveChangesAsync();
    return tarefa;
}

    public async Task<Tarefa?> ConcluirAsync(int id)
    {
        var tarefa = await _db.Tarefas.FindAsync(id);
        if (tarefa is null)
        {
            return null;
        }

        tarefa.Concluida = !tarefa.Concluida;
        await _db.SaveChangesAsync();
        return tarefa;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var tarefa = await _db.Tarefas.FindAsync(id);
        if (tarefa is null)
        {
            return false;
        }

        _db.Tarefas.Remove(tarefa);
        await _db.SaveChangesAsync();
        return true;
    }
}
