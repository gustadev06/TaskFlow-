using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);


var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}


var connectionString = DbConnectionHelper.Resolver(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddHttpClient<QuoteService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();


var tarefas = app.MapGroup("/api/tarefas");

tarefas.MapGet("/", async (ITarefaService svc) =>
    Results.Ok(await svc.ListarAsync()));

tarefas.MapGet("/estatisticas", async (ITarefaService svc) =>
    Results.Ok(await svc.ObterEstatisticasAsync()));

tarefas.MapGet("/{id:int}", async (int id, ITarefaService svc) =>
{
    var tarefa = await svc.ObterAsync(id);
    return tarefa is null ? Results.NotFound() : Results.Ok(tarefa);
});

tarefas.MapPost("/", async (CriarTarefaDto dto, ITarefaService svc) =>
{
    try
    {

        var tarefa = await svc.CriarAsync(dto.Titulo, dto.Prioridade);
        return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { erro = ex.Message });
    }
});

tarefas.MapPut("/{id:int}", async (int id, AtualizarTarefaDto dto, ITarefaService svc) =>
{

    var tarefa = await svc.AtualizarAsync(id, dto.Titulo, dto.Prioridade, dto.Concluida);
    return tarefa is null ? Results.NotFound() : Results.Ok(tarefa);
});

tarefas.MapPatch("/{id:int}/concluir", async (int id, ITarefaService svc) =>
{
    var tarefa = await svc.ConcluirAsync(id);
    return tarefa is null ? Results.NotFound() : Results.Ok(tarefa);
});

tarefas.MapDelete("/{id:int}", async (int id, ITarefaService svc) =>
{
    var removida = await svc.RemoverAsync(id);
    return removida ? Results.NoContent() : Results.NotFound();
});


app.MapGet("/api/frase-do-dia", async (QuoteService svc) =>
    Results.Ok(await svc.ObterFraseDoDiaAsync()));


app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();


public record CriarTarefaDto(string Titulo, string Prioridade);
public record AtualizarTarefaDto(string Titulo, string Prioridade, bool Concluida);