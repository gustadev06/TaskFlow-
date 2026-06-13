using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Services;

namespace TaskFlow.Tests;

// Testes de integracao da camada de persistencia (EF Core InMemory).
public class TarefaServiceTests
{
    private static AppDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CriarAsync_TituloValido_DevePersistirNoBanco()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);

        // CORRIGIDO: Passando a prioridade
        var tarefa = await service.CriarAsync("Revisar POO", "Alta");

        Assert.True(tarefa.Id > 0);
        Assert.Single(await service.ListarAsync());
    }

    [Fact]
    public async Task CriarAsync_TituloVazio_DeveLancarExcecao()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);

        // CORRIGIDO: Passando a prioridade
        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync("", "Média"));
    }

    [Fact]
    public async Task ConcluirAsync_TarefaExistente_DeveAlternarStatus()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);
        
        // CORRIGIDO: Passando a prioridade
        var tarefa = await service.CriarAsync("Ler Constituicao", "Alta");

        var atualizada = await service.ConcluirAsync(tarefa.Id);

        Assert.NotNull(atualizada);
        Assert.True(atualizada!.Concluida);
    }

    [Fact]
    public async Task RemoverAsync_IdInexistente_DeveRetornarFalse()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);
        
        // CORRIGIDO: Passando a prioridade
        await service.CriarAsync("Tarefa qualquer", "Baixa");

        var resultado = await service.RemoverAsync(999);

        Assert.False(resultado);
        Assert.Single(await service.ListarAsync());
    }

    [Fact]
    public async Task AtualizarAsync_TarefaExistente_DeveAlterarTitulo()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);
        
        // CORRIGIDO: Passando a prioridade
        var tarefa = await service.CriarAsync("Titulo antigo", "Média");

        // CORRIGIDO: Passando id, titulo, prioridade e status de concluida
        var atualizada = await service.AtualizarAsync(tarefa.Id, "Titulo novo", "Alta", true);

        Assert.NotNull(atualizada);
        Assert.Equal("Titulo novo", atualizada!.Titulo);
        Assert.True(atualizada.Concluida);
    }
}