using System.Text.Json.Serialization;

namespace TaskFlow.Models;

public class TranslationResponse
{
    [JsonPropertyName("responseData")]
    public TranslationData? ResponseData { get; set; }
}

public class TranslationData
{
    [JsonPropertyName("translatedText")]
    public string TranslatedText { get; set; } = string.Empty;
}