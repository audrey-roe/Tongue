﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Tongue.Models;
using Tongue.Services;

namespace Tongue.ViewModels
{
    public sealed partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private long _sourceCharCount;

        [ObservableProperty]
        private long _translationCharCount;

        [ObservableProperty]
        private string _sourceText;

        [ObservableProperty]
        private string _translatedText;

        [ObservableProperty]
        private LanguageInfo _selectedSourceLangInfo;

        [ObservableProperty]
        private LanguageInfo _selectedTranslationLangInfo;

        public ObservableCollection<TranslationHistory> TranslationHistory { get; } = new();

        [RelayCommand]
        public async Task GetTranslationHistoryAsync()
        {
            foreach (var item in await Ioc.Default.GetRequiredService<IRepositoryService>().GetSavedTranslationsAsync())
            {
                TranslationHistory.Add(item);
            }
        }

        [RelayCommand]
        public async Task ClearTranslationHistoryAsync()
        {
            await Ioc.Default.GetRequiredService<IRepositoryService>().ClearHistoryAsync();
            TranslationHistory.Clear();
        }

        [RelayCommand]
        public async Task RemoveHistoryItemAsync(TranslationHistory history)
        {
            await Ioc.Default.GetRequiredService<IRepositoryService>().DeleteHistoryItemAsync(history);
            TranslationHistory.Remove(history);
        }

        [RelayCommand]
        private async Task TranslateAsync(bool isButton)
        {
            if (string.IsNullOrWhiteSpace(SourceText)) return;

            TranslatedText = await Ioc.Default
                .GetRequiredService<ITranslationService>()
                .TranslateAsync(SourceText, SelectedSourceLangInfo.LanguageCode, SelectedTranslationLangInfo.LanguageCode);

            if (isButton)
            {
                var item = new TranslationHistory()
                {
                    SourceText = SourceText,
                    TranslatedText = TranslatedText,
                    SourceLanguage = SelectedSourceLangInfo.LanguageCode,
                    TranslationLanguage = SelectedTranslationLangInfo.LanguageCode,
                    Date = DateTime.UtcNow
                };

                TranslationHistory.Add(item);

                await Ioc.Default.GetRequiredService<IRepositoryService>().AddSavedTranslationAsync(item);
            }
        }
    }
}
