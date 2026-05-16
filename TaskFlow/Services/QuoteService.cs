using System.Net.Http.Json;
using TaskFlow.Models;

namespace TaskFlow.Services;

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
            // 1. Busca a frase do dia em inglês na ZenQuotes
            var url = "https://zenquotes.io/api/today";
            var quotes = await _http.GetFromJsonAsync<List<Quote>>(url);

            if (quotes != null && quotes.Count > 0)
            {
                var quote = quotes[0];

                // 2. Traduz a frase pro português usando a MyMemory
                quote.Texto = await TraduzirAsync(quote.Texto);
                return quote;
            }
        }
        catch
        {
            // Cai no fallback se a ZenQuotes falhar
        }

        return new Quote
        {
            Texto = "A disciplina é a ponte entre metas e conquistas.",
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
                return traducao;
        }
        catch
        {
            // Se a tradução falhar, devolve o texto original em inglês
        }

        return textoEmIngles;
    }
}