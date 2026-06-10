using System.Text.Json.Serialization;

namespace TaskFlow.Api.Models;

public class Quote
{
    [JsonPropertyName("q")]
    public string Texto { get; set; } = string.Empty;

    [JsonPropertyName("a")]
    public string Autor { get; set; } = string.Empty;
}
