using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tongue.Models;

namespace Tongue.Services
{
    public interface IRepositoryService
    {
        Task InitializeAsync();

        Task<IList<TranslationHistory>> GetSavedTranslationsAsync();

        Task<TranslationHistory> GetSavedTranslationAsync(Guid id);

        Task DeleteHistoryItemAsync(TranslationHistory item);

        Task AddSavedTranslationAsync(TranslationHistory item);

        Task ClearHistoryAsync();
    }
}
