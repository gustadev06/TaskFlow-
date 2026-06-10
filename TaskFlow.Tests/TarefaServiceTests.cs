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

        var tarefa = await service.CriarAsync("Revisar POO");

        Assert.True(tarefa.Id > 0);
        Assert.Single(await service.ListarAsync());
    }

    [Fact]
    public async Task CriarAsync_TituloVazio_DeveLancarExcecao()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync(""));
    }

    [Fact]
    public async Task ConcluirAsync_TarefaExistente_DeveAlternarStatus()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);
        var tarefa = await service.CriarAsync("Ler Constituicao");

        var atualizada = await service.ConcluirAsync(tarefa.Id);

        Assert.NotNull(atualizada);
        Assert.True(atualizada!.Concluida);
    }

    [Fact]
    public async Task RemoverAsync_IdInexistente_DeveRetornarFalse()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);
        await service.CriarAsync("Tarefa qualquer");

        var resultado = await service.RemoverAsync(999);

        Assert.False(resultado);
        Assert.Single(await service.ListarAsync());
    }

    [Fact]
    public async Task AtualizarAsync_TarefaExistente_DeveAlterarTitulo()
    {
        using var db = CriarContextoEmMemoria();
        var service = new TarefaService(db);
        var tarefa = await service.CriarAsync("Titulo antigo");

        var atualizada = await service.AtualizarAsync(tarefa.Id, "Titulo novo", true);

        Assert.NotNull(atualizada);
        Assert.Equal("Titulo novo", atualizada!.Titulo);
        Assert.True(atualizada.Concluida);
    }
}
