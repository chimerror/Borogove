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
        public DbSet<CreatorInfoEntity> Creators { get; set; }
        public DbSet<WorkCreatorEntity> WorkCreators { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

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

            var aliasEntityModel = modelBuilder.Entity<AliasEntity>()
                .HasKey(a => a.Alias)
                .ToTable("Aliases");

            var creatorInfoEntityModel = modelBuilder.Entity<CreatorInfoEntity>()
                .HasKey(ci => ci.Name)
                .ToTable("Creators");
            creatorInfoEntityModel
                .HasMany(ci => ci.Aliases)
                .WithRequired()
                .HasForeignKey(a => a.AliasOf);

            var workCreatorEntityModel = modelBuilder.Entity<WorkCreatorEntity>()
                .HasKey(wce => new
                {
                    wce.WorkIdentifier,
                    wce.CreatorName,
                    wce.Role,
                    wce.WorkedAsName,
                })
                .Ignore(wce => wce.WorkedAs)
                .ToTable("WorkCreatorEntities");
            workCreatorEntityModel.Property(wce => wce.WorkIdentifier)
                .IsRequired()
                .HasColumnName("Work");
            workCreatorEntityModel.Property(wce => wce.CreatorName)
                .IsRequired()
                .HasColumnName("Creator");
            workCreatorEntityModel.Property(wce => wce.WorkedAsName)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("WorkedAs");
            workCreatorEntityModel
                .HasRequired(wce => wce.Work)
                .WithMany();
            workCreatorEntityModel
                .HasRequired(wce => wce.Creator)
                .WithMany();

            var tagAliasEntityModel = modelBuilder.Entity<TagAliasEntity>()
                .HasKey(ta => ta.Alias)
                .ToTable("TagAliases");

            var tagEntityModel = modelBuilder.Entity<TagEntity>()
                .HasKey(t => t.TagName)
                .ToTable("Tags");
            tagEntityModel
                .HasMany(t => t.Aliases)
                .WithRequired()
                .HasForeignKey(ta => ta.TagName)
                .WillCascadeOnDelete(true);
            tagEntityModel
                .HasMany(t => t.Implications)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("Tag");
                    m.MapRightKey("Implication");
                    m.ToTable("TagImplications");
                });
        }
    }
}
