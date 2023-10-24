using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tongue.Models;

namespace Tongue.Services
{
    public sealed class LibreTranslateService : ITranslationService, IDisposable
    {
        private HttpClient _httpClient;

        private IList<LanguageInfo> _supportedSourceLanguages;
        private IList<LanguageInfo> _supportedTranslationLanguages;

        public IList<LanguageInfo> SupportedSourceLanguages
        {
            get
            {
                return _supportedSourceLanguages ??= new List<LanguageInfo>()
                {
                    new("Auto", "auto"),
                    new("English", "en"),
                    new("Arabic", "ar"),
                    new("Azerbaijani", "az"),
                    new("Chinese", "zh"),
                    new("Czech", "cs"),
                    new("Danish", "da"),
                    new("Dutch", "nl"),
                    new("Esperanto", "eo"),
                    new("Finnish", "fi"),
                    new("French", "fr"),
                    new("German", "de"),
                    new("Greek", "el"),
                    new("Hebrew", "he"),
                    new("Hindi", "hi"),
                    new("Hungarian", "hu"),
                    new("Indonesian", "id"),
                    new("Irish", "ga"),
                    new("Italian", "it"),
                    new("Japanese", "ja"),
                    new("Korean", "ko"),
                    new("Persian", "fa"),
                    new("Polish", "pl"),
                    new("Portuguese", "pt"),
                    new("Russian", "ru"),
                    new("Slovak", "sk"),
                    new("Spanish", "es"),
                    new("Swedish", "sv"),
                    new("Turkish", "tr"),
                    new("Ukranian", "uk")
                };
            }
        }

        public IList<LanguageInfo> SupportedTranslationLanguages
        {
            get
            {
                if (_supportedTranslationLanguages == null)
                {
                    var clonedList = SupportedSourceLanguages.ToList();
                    clonedList.RemoveAt(0);

                    _supportedTranslationLanguages = clonedList;
                }

                return _supportedTranslationLanguages;
            }
        }

        public async Task<string> TranslateAsync(string text, string fromLocale, string toLocale)
        {
            _httpClient ??= new HttpClient();

            var info = new LibreTranslationInfo()
            {
                Query = text,
                Format = "text",
                Source = fromLocale,
                Target = toLocale
            };

            var content = new StringContent(JsonSerializer.Serialize(info), System.Text.Encoding.UTF8, "application/json");

            int maxRetryCount = 3; // Maximum number of retries
            int retryCount = 0;

            while (true)
            {
                try
                {
                    var response = await _httpClient.PostAsync("https://libretranslate.org/translate", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var translationResponse = JsonSerializer.Deserialize<LibreTranslationResponse>(responseContent);

                        return translationResponse.TranslatedText;
                    }
                    else if (retryCount < maxRetryCount)
                    {
                        // Retry after a delay (you can adjust the delay as needed).
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        retryCount++;
                    }
                    else
                    {
                        // Handle the case when all retries fail, you can log or throw an exception.
                        throw new Exception($"Failed to translate after {maxRetryCount} retries.");
                    }
                }
                catch (HttpRequestException)
                {
                    if (retryCount < maxRetryCount)
                    {
                        // Retry after a delay (you can adjust the delay as needed).
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        retryCount++;
                    }
                    else
                    {
                        // Handle the case when all retries fail, you can log or throw an exception.
                        throw new Exception($"Failed to translate after {maxRetryCount} retries.");
                    }
                }
            }
        }



        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
