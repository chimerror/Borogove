using System.Data.Entity;
using System.Globalization;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkContext : DbContext
    {
        public DbSet<WorkEntity> Works { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Work>();
            modelBuilder.Ignore<Creator>();
            modelBuilder.Ignore<CultureInfo>();
            modelBuilder.Ignore<Tag>();
            modelBuilder.Entity<WorkEntity>()
                .HasKey(w => w.Identifier)
                .Ignore(w => w.Language)
                .Ignore(w => w.Tags)
                .Ignore(w => w.Creators);
            modelBuilder.Entity<CreatorInfoEntity>()
                .HasKey(c => c.Name);
            var workCreatorEntityModel = modelBuilder.Entity<WorkCreatorEntity>()
                .HasKey(wc => new { wc.WorkIdentifier, wc.Role, wc.CreatorName, wc.WorkedAs });
        }
    }
}
