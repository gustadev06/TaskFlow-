using System;
using Xunit;
using TaskFlow;

namespace TaskFlow.Tests;

public class GerenciadorDeTarefasTests
{
    // Teste 1
    [Fact]
    public void AdicionarMeta_NomeValido_DeveAdicionarComSucesso()
    {
        var gerenciador = new GerenciadorDeTarefas();
        
        gerenciador.Adicionar("Revisar POO");

        Assert.Single(gerenciador.ListarMetas()); 
    }

    // Teste 2
    [Fact]
    public void AdicionarMeta_NomeVazio_DeveGerarErro()
    {
        var gerenciador = new GerenciadorDeTarefas();
        
      
        Assert.Throws<ArgumentException>(() => gerenciador.Adicionar(""));
    }

    // Teste 3
    [Fact]
    public void RemoverMeta_IdInexistente_NaoDeveQuebrarOSistema()
    {
        var gerenciador = new GerenciadorDeTarefas();
        gerenciador.Adicionar("Ler Constituição");

        bool resultado = gerenciador.Remover(99);

        Assert.False(resultado); 
        Assert.Single(gerenciador.ListarMetas()); 
    }
}