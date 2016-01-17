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
        public DbSet<CreatorAliasEntity> CreatorAliases { get; set; }
        public DbSet<WorkCreatorEntity> WorkCreators { get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TagAliasEntity> TagAliases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Creator>();
            modelBuilder.Ignore<CultureInfo>();
            modelBuilder.Ignore<Tag>();

            var workEntityModel = modelBuilder.Entity<WorkEntity>()
                .HasKey(w => w.Identifier)
                .Ignore(w => w.Tags)
                .Ignore(w => w.Creators)
                .Ignore(w => w.Language)
                .ToTable("Works");
            workEntityModel
                .HasMany(w => w.WorkCreators)
                .WithRequired(w => w.Work);
            workEntityModel
                .HasOptional(w => w.LanguageEntity)
                .WithMany()
                .Map(m => m.MapKey("Language"));
            workEntityModel
                .HasMany(w => w.TagEntities)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("Work");
                    m.MapRightKey("Tag");
                    m.ToTable("WorkTags");
                });
            workEntityModel
                .HasOptional(w => w.ParentEntity)
                .WithMany(w => w.ChildrenEntities)
                .Map(m => m.MapKey("Parent"));
            workEntityModel
                .HasMany(w => w.NextWorkEntities)
                .WithMany(w => w.PreviousWorkEntities)
                .Map(m =>
                {
                    m.MapLeftKey("Work");
                    m.MapRightKey("NextWork");
                    m.ToTable("NextWorks");
                });
            workEntityModel
                .HasOptional(w => w.DraftOfEntity)
                .WithMany(w => w.DraftEntities)
                .Map(m => m.MapKey("DraftOf"));
            workEntityModel
                .HasOptional(w => w.ArtifactOfEntity)
                .WithMany(w => w.ArtifactEntities)
                .Map(m => m.MapKey("ArtifiactOf"));
            workEntityModel
                .HasOptional(w => w.CommentsOnEntity)
                .WithMany(w => w.CommentEntities)
                .Map(m => m.MapKey("CommentsOn"));

            var creatorAliasEntityModel = modelBuilder.Entity<CreatorAliasEntity>()
                .HasKey(a => a.Alias)
                .ToTable("CreatorAliases");

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
                .ToTable("WorkCreators");
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

            var languageEntityModel = modelBuilder.Entity<LanguageEntity>()
                .HasKey(l => l.Name)
                .ToTable("Languages");

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
