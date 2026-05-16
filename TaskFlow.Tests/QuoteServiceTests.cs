using System.Net;
using System.Text;
using TaskFlow.Services;
using Xunit;

namespace TaskFlow.Tests;

// Handler falso: devolve JSONs diferentes dependendo da URL chamada
public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, string> _respostas;
    private readonly HttpStatusCode _status;

    public FakeHttpMessageHandler(Dictionary<string, string> respostas, HttpStatusCode status = HttpStatusCode.OK)
    {
        _respostas = respostas;
        _status = status;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var url = request.RequestUri!.ToString();
        var jsonRetorno = "";

        // Decide qual resposta devolver com base na URL
        foreach (var par in _respostas)
        {
            if (url.Contains(par.Key))
            {
                jsonRetorno = par.Value;
                break;
            }
        }

        var response = new HttpResponseMessage(_status)
        {
            Content = new StringContent(jsonRetorno, Encoding.UTF8, "application/json")
        };
        return Task.FromResult(response);
    }
}

public class QuoteServiceTests
{
    [Fact]
    public async Task ObterFraseDoDia_ComRespostaValida_DeveDesserializarETraduzir()
    {
        // Arrange: simula as duas APIs
        var respostas = new Dictionary<string, string>
        {
            ["zenquotes.io"] = """
                [ { "q": "Discipline is the bridge.", "a": "Jim Rohn" } ]
                """,
            ["mymemory.translated.net"] = """
                { "responseData": { "translatedText": "Disciplina é a ponte." } }
                """
        };
        var handler = new FakeHttpMessageHandler(respostas);
        var service = new QuoteService(new HttpClient(handler));

        // Act
        var frase = await service.ObterFraseDoDiaAsync();

        // Assert: a frase final deve estar traduzida, o autor preservado
        Assert.Equal("Disciplina é a ponte.", frase.Texto);
        Assert.Equal("Jim Rohn", frase.Autor);
    }

    [Fact]
    public async Task ObterFraseDoDia_ComFalhaNaApi_DeveRetornarFraseFallback()
    {
        // Arrange: simula as duas APIs fora do ar
        var respostas = new Dictionary<string, string>();
        var handler = new FakeHttpMessageHandler(respostas, HttpStatusCode.InternalServerError);
        var service = new QuoteService(new HttpClient(handler));

        // Act
        var frase = await service.ObterFraseDoDiaAsync();

        // Assert: o app não quebra, usa o fallback
        Assert.False(string.IsNullOrWhiteSpace(frase.Texto));
        Assert.False(string.IsNullOrWhiteSpace(frase.Autor));
    }
}