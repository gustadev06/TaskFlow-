using System.Net.Http.Json;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services;

// Consome duas APIs externas: ZenQuotes (frase do dia) + MyMemory (traducao).
public class QuoteService
{
    private readonly HttpClient _http;

    public QuoteService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Quote> ObterFraseDoDiaAsync()
    {
        try
        {
            var url = "https://zenquotes.io/api/today";
            var quotes = await _http.GetFromJsonAsync<List<Quote>>(url);

            if (quotes != null && quotes.Count > 0)
            {
                var quote = quotes[0];
                quote.Texto = await TraduzirAsync(quote.Texto);
                return quote;
            }
        }
        catch
        {
            // Cai no fallback se a ZenQuotes falhar.
        }

        return new Quote
        {
            Texto = "A disciplina e a ponte entre metas e conquistas.",
            Autor = "Jim Rohn"
        };
    }

    private async Task<string> TraduzirAsync(string textoEmIngles)
    {
        try
        {
            var textoCodificado = Uri.EscapeDataString(textoEmIngles);
            var url = $"https://api.mymemory.translated.net/get?q={textoCodificado}&langpair=en|pt-br";

            var resposta = await _http.GetFromJsonAsync<TranslationResponse>(url);
            var traducao = resposta?.ResponseData?.TranslatedText;

            if (!string.IsNullOrWhiteSpace(traducao))
            {
                return traducao;
            }
        }
        catch
        {
            // Se a traducao falhar, devolve o texto original.
        }

        return textoEmIngles;
    }
}
