# TaskFlow — Foco na Aprovação

🔗 **Download da aplicação:** [Release v1.0.0](https://github.com/gustadev06/TaskFlow-/releases/tag/v1.0.0)

Organizador de estudos via linha de comando, com frase motivacional do dia integrada às APIs públicas **ZenQuotes** (frases) e **MyMemory** (tradução para português).

## Funcionalidades

- ➕ Adicionar tarefas de estudo
- 📋 Listar tarefas pendentes
- ✅ Concluir / remover tarefas
- 💬 Frase motivacional do dia em português (atualizada a cada execução)

## Como executar

### Opção 1 — Baixar o executável
Acesse a [Release v1.0.0](https://github.com/gustadev06/TaskFlow-/releases/tag/v1.0.0), baixe o `TaskFlow.exe` e execute pelo terminal:

```bash
.\TaskFlow.exe
```

### Opção 2 — Executar a partir do código-fonte

```bash
git clone https://github.com/gustadev06/TaskFlow-.git
cd TaskFlow-
dotnet run --project TaskFlow
```

## Tecnologias

- **C# / .NET 10** — linguagem e runtime
- **xUnit** — testes unitários e de integração
- **GitHub Actions** — pipeline de CI (lint + build + testes)
- **APIs públicas:**
  - [ZenQuotes](https://zenquotes.io/) — frase motivacional do dia
  - [MyMemory](https://mymemory.translated.net/) — tradução para PT-BR

## Estrutura do projeto

```
TaskFlow Bottcamp/
├── TaskFlow/              # Aplicação principal (CLI)
│   ├── Models/            # Quote, TranslationResponse
│   ├── Services/          # QuoteService
│   └── Program.cs         # Gerenciador de tarefas + menu
├── TaskFlow.Tests/        # Testes (xUnit)
└── .github/workflows/     # Pipeline de CI
```
