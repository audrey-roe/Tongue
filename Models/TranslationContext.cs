using Microsoft.EntityFrameworkCore;

namespace Tongue.Models
{
    public class TranslationContext : DbContext
    {
        public DbSet<TranslationHistory> TranslationHistory { get; set; }

        public TranslationContext(DbContextOptions<TranslationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define data model and relationships here
            modelBuilder.Entity<TranslationHistory>().HasKey(th => th.Id);
        }
    }
}
