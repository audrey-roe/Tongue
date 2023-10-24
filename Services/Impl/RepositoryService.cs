using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tongue.Models;

namespace Tongue.Services
{
    public sealed class RepositoryService : IRepositoryService
    {
        private TranslationContext _context;

        public async Task InitializeAsync()
        {
            var databasePath = "Database.db";
            var connectionString = new SqliteConnectionStringBuilder { DataSource = databasePath }.ToString();

            var options = new DbContextOptionsBuilder<TranslationContext>()
                .UseSqlite(connectionString)
                .Options;

            _context = new TranslationContext(options);

            await _context.Database.EnsureCreatedAsync();
        }


        public async Task DeleteHistoryItemAsync(TranslationHistory item)
        {
            _context.TranslationHistory.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TranslationHistory> GetSavedTranslationAsync(Guid id)
        {
            return await _context.TranslationHistory.FindAsync(id);
        }

        public async Task<IList<TranslationHistory>> GetSavedTranslationsAsync()
        {
            return await _context.TranslationHistory.ToListAsync();
        }

        public async Task AddSavedTranslationAsync(TranslationHistory item)
        {
            await _context.TranslationHistory.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task ClearHistoryAsync()
        {
            _context.TranslationHistory.RemoveRange(_context.TranslationHistory);
            await _context.SaveChangesAsync();
        }
    }
}
