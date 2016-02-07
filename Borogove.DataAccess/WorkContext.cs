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

        public TagSet GetTagSet()
        {
            var tagSet = new TagSet();

            foreach (TagEntity tag in Tags)
            {
                tagSet.ResolveTag(tag.TagName, true);
            }

            foreach (TagAliasEntity tagAlias in TagAliases)
            {
                tagSet.ResolveTag($"{tagAlias.Alias}{TagSet.AliasSeparator}{tagAlias.TagName}", true);
            }

            foreach (TagEntity tag in Tags)
            {
                foreach (TagEntity implication in tag.Implications)
                {
                    tagSet.ResolveTag($"{tag.TagName}{TagSet.ImplicationSeparator}{implication.TagName}", true);
                }
            }

            return tagSet;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<Creator>();
            modelBuilder.Ignore<CultureInfo>();
            modelBuilder.Ignore<Tag>();

            var workEntityModel = modelBuilder.Entity<WorkEntity>()
                .HasKey(w => w.Identifier)
                .ToTable("Works");
            workEntityModel
                .HasMany(w => w.WorkCreators)
                .WithRequired(wce => wce.Work);
            workEntityModel
                .HasOptional(w => w.Language)
                .WithMany();
            workEntityModel
                .HasMany(w => w.Tags)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("Work");
                    m.MapRightKey("Tag");
                    m.ToTable("WorkTags");
                });
            workEntityModel
                .HasOptional(w => w.Parent)
                .WithMany(w => w.Children);
            workEntityModel
                .HasMany(w => w.NextWorks)
                .WithMany(w => w.PreviousWorks)
                .Map(m =>
                {
                    m.MapLeftKey("Work");
                    m.MapRightKey("NextWork");
                    m.ToTable("NextWorks");
                });
            workEntityModel
                .HasOptional(w => w.DraftOf)
                .WithMany(w => w.Drafts);
            workEntityModel
                .HasOptional(w => w.ArtifactOf)
                .WithMany(w => w.Artifacts);
            workEntityModel
                .HasOptional(w => w.CommentsOn)
                .WithMany(w => w.Comments);

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
                .ToTable("WorkCreators");
            workCreatorEntityModel.Property(wce => wce.WorkIdentifier)
                .IsRequired()
                .HasColumnName("Work");
            workCreatorEntityModel.Property(wce => wce.CreatorName)
                .IsRequired()
                .HasColumnName("Creator");
            workCreatorEntityModel.Property(wce => wce.WorkedAsName)
                .IsRequired()
                .HasColumnName("WorkedAs");
            workCreatorEntityModel
                .HasRequired(wce => wce.Work)
                .WithMany(w => w.WorkCreators)
                .HasForeignKey(wce => wce.WorkIdentifier);
            workCreatorEntityModel
                .HasRequired(wce => wce.Creator)
                .WithMany()
                .HasForeignKey(wce => wce.CreatorName);

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
