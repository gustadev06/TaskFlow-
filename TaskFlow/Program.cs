using System;
using System.Collections.Generic;

namespace TaskFlow;

// --- O CÉREBRO DO APLICATIVO (Testável) ---
public class GerenciadorDeTarefas
{
    private List<string> tarefas = new List<string>();

    public void Adicionar(string novaTarefa)
    {
        if (string.IsNullOrWhiteSpace(novaTarefa))
        {
            throw new ArgumentException("A tarefa não pode ser vazia.");
        }
        tarefas.Add(novaTarefa);
    }

    public List<string> ListarMetas()
    {
        return tarefas; // Retorna a lista para quem pedir
    }

    public bool Remover(int numeroDigitado)
    {
        int indice = numeroDigitado - 1; // O computador conta a partir do 0
        if (indice >= 0 && indice < tarefas.Count)
        {
            tarefas.RemoveAt(indice);
            return true; // Sucesso ao remover
        }
        return false; // Falhou ao remover (número inválido)
    }
}

// --- A TELA DO APLICATIVO (O que o usuário vê) ---
class Program
{
    static void Main(string[] args)
    {
        GerenciadorDeTarefas gerenciador = new GerenciadorDeTarefas();
        bool rodando = true;

        while (rodando)
        {
            Console.Clear();
            Console.WriteLine("=== TaskFlow - Foco na Aprovação ===");
            Console.WriteLine("1. Adicionar tarefa (Ex: ler regulamentação do SUS)");
            Console.WriteLine("2. Listar tarefas");
            Console.WriteLine("3. Concluir / Remover tarefa");
            Console.WriteLine("4. Sair");
            Console.Write("\nEscolha uma opção: ");

            string opcao = Console.ReadLine() ?? "";

            if (opcao == "1")
            {
                Console.Write("Digite a nova tarefa: ");
                string novaTarefa = Console.ReadLine() ?? "";

                try
                {
                    gerenciador.Adicionar(novaTarefa);
                    Console.WriteLine("Tarefa adicionada com sucesso!");
                }
                catch (Exception ex)
                {
                    // Se tentar adicionar vazio, o sistema não quebra, só avisa.
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else if (opcao == "2")
            {
                Console.WriteLine("\n--- Suas Tarefas ---");
                var lista = gerenciador.ListarMetas();
                for (int i = 0; i < lista.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {lista[i]}");
                }
            }
            else if (opcao == "3")
            {
                Console.WriteLine("\n--- Remover tarefa ---");
                var lista = gerenciador.ListarMetas();
                for (int i = 0; i < lista.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {lista[i]}");
                }

                Console.Write("\nDigite o numero da tarefa concluida: ");
                if (int.TryParse(Console.ReadLine(), out int numero))
                {
                    bool sucesso = gerenciador.Remover(numero);
                    if (sucesso)
                    {
                        Console.WriteLine("Tarefa removida da lista!");
                    }
                    else
                    {
                        Console.WriteLine("Numero invalido.");
                    }
                }
            }
            else if (opcao == "4")
            {
                rodando = false;
                Console.WriteLine("Saindo... Bons estudos!");
            }

            if (rodando)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}