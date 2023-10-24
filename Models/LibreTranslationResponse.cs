#nullable enable

using System.Text.Json.Serialization;

namespace Tongue.Models
{
    public sealed class LibreTranslationResponse
    {
        [JsonPropertyName("detectedLanguage")]
        public LibreDetectedLanguageInfo? DetectedLanguageInfo { get; set; }

        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; } = string.Empty;
    }
}

