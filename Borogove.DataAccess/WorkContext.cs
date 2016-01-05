using System.Data.Entity;
using System.Globalization;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkContext : DbContext
    {
        public WorkContext() : base("Borogove") { }

        public DbSet<WorkEntity> Works { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Creator>();
            modelBuilder.Ignore<Tag>();
            modelBuilder.Ignore<CultureInfo>();
            modelBuilder.Ignore<Work>();

            var languageEntityModel = modelBuilder.Entity<LanguageEntity>()
                .HasKey(l => l.Name)
                .ToTable("Languages");

            var workEntityModel = modelBuilder.Entity<WorkEntity>()
                .HasKey(w => w.Identifier)
                .Ignore(w => w.Tags)
                .Ignore(w => w.Creators)
                .Ignore(w => w.Language)
                .ToTable("Works");
            workEntityModel
                .HasOptional(w => w.LanguageEntity)
                .WithMany()
                .Map(m => m.MapKey("Language"));
        }
    }
}
