# TaskFlow - Foco na Aprovação 🎯

![CI](https://github.com/gustadev06/TaskFlow-/actions/workflows/ci.yml/badge.svg)
**Versão:** 2.0.0

> Aplicação web para fatiar editais e legislações densas em **micro-metas** de estudo,
> com persistência em **banco de dados na nuvem (Supabase / PostgreSQL)**.

---

## 👥 Integrantes do Grupo

| Nome completo | Matrícula | Usuário GitHub |
|---|---|---|
| Gustavo (dono do repo) | _preencher_ | [@gustadev06](https://github.com/gustadev06) |
| _preencher_ | _preencher_ | _preencher_ |
| _preencher_ | _preencher_ | _preencher_ |
| _preencher_ | _preencher_ | _preencher_ |
| _preencher_ | _preencher_ | _preencher_ |

## 🔗 Links

- **Repositório:** https://github.com/gustadev06/TaskFlow-
- **Aplicação publicada (Deploy):** _preencher após o deploy_
- **Documentação da API (Swagger):** `{link-do-deploy}/swagger`

---

## 🚨 O Problema

Estudantes e concurseiros lidam com editais densos e legislações extensas. Esse volume gera
sobrecarga e dificulta a constância. O **TaskFlow** quebra esses blocos em tarefas diárias
gerenciáveis, reduzindo a fadiga de decisão.

## 🚀 Funcionalidades

- **Adicionar** metas de estudo (persistidas no banco)
- **Listar** tarefas (pendentes primeiro, mais recentes no topo)
- **Concluir / reabrir** uma tarefa
- **Editar** e **remover** tarefas
- **Frase motivacional do dia** consumida de uma API externa (ZenQuotes + tradução MyMemory)

## 🛠️ Stack

| Camada | Tecnologia |
|---|---|
| Linguagem | C# / .NET 10 |
| API | ASP.NET Core (Minimal APIs) |
| **Banco de dados** | **Supabase (PostgreSQL)** via Entity Framework Core + Npgsql |
| Documentação | Swagger / OpenAPI (Swashbuckle) |
| Front-end | HTML + CSS + JavaScript (estático) |
| Testes | xUnit + EF Core InMemory |
| CI | GitHub Actions (`dotnet format` + `build` + `test`) |
| Deploy | Docker (Render / Railway) |

---

## 💻 Como rodar localmente

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Uma conta no [Supabase](https://supabase.com) (gratuita)

### 1. Pegue a connection string no Supabase
No painel do projeto: **Project Settings → Database → Connection string → "Session pooler"**.
Ela tem este formato:
```
postgresql://postgres.SEU_REF:SUA_SENHA@aws-0-sa-east-1.pooler.supabase.com:5432/postgres
```

### 2. Configure o segredo localmente
Crie o arquivo `TaskFlow.Api/appsettings.Development.json` (já está no `.gitignore`,
**não vai pro GitHub**) a partir do exemplo:
```bash
cp TaskFlow.Api/appsettings.Development.example.json TaskFlow.Api/appsettings.Development.json
```
E cole sua connection string no campo `DefaultConnection`.

> Alternativa: `export DATABASE_URL="postgresql://..."` antes de rodar.

### 3. Rode
```bash
dotnet restore TaskFlow.slnx
dotnet run --project TaskFlow.Api
```
Acesse:
- App: http://localhost:5000 (ou a porta exibida no terminal)
- Swagger: http://localhost:5000/swagger

A tabela `Tarefas` é criada automaticamente no primeiro start (`EnsureCreated`).

### 4. Rodar os testes
```bash
dotnet test TaskFlow.slnx
```

### 5. Antes de abrir um PR (importante p/ o CI ficar verde)
```bash
dotnet format TaskFlow.slnx
```

---

## ☁️ Deploy (Render via Docker)

1. No [Render](https://render.com): **New → Web Service** → conecte o repositório.
2. Render detecta o `Dockerfile` automaticamente (Runtime = Docker).
3. Em **Environment**, adicione a variável:
   - `DATABASE_URL` = sua connection string do Supabase
4. Deploy. O endpoint `/health` é usado como health check.

> Funciona igual no Railway (Docker) — basta setar a mesma variável `DATABASE_URL`.

---

## 📡 Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/tarefas` | Lista todas |
| GET | `/api/tarefas/{id}` | Busca uma |
| POST | `/api/tarefas` | Cria `{ "titulo": "..." }` |
| PUT | `/api/tarefas/{id}` | Atualiza `{ "titulo": "...", "concluida": true }` |
| PATCH | `/api/tarefas/{id}/concluir` | Alterna concluída |
| DELETE | `/api/tarefas/{id}` | Remove |
| GET | `/api/frase-do-dia` | Frase motivacional (API externa) |
| GET | `/health` | Status do serviço |
