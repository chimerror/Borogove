using System.Data.Entity.Migrations;

namespace Borogove.DataAccess.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreatorAliases",
                c => new
                    {
                        Alias = c.String(nullable: false, maxLength: 128),
                        AliasOf = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Creators", t => t.AliasOf, cascadeDelete: true)
                .Index(t => t.AliasOf);

            CreateTable(
                "dbo.Creators",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);

            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);

            CreateTable(
                "dbo.TagAliases",
                c => new
                    {
                        Alias = c.String(nullable: false, maxLength: 128),
                        TagName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Alias)
                .ForeignKey("dbo.Tags", t => t.TagName, cascadeDelete: true)
                .Index(t => t.TagName);

            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.TagName);

            CreateTable(
                "dbo.WorkCreators",
                c => new
                    {
                        Work = c.Guid(nullable: false),
                        Creator = c.String(nullable: false, maxLength: 128),
                        Role = c.Int(nullable: false),
                        WorkedAs = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Work, t.Creator, t.Role, t.WorkedAs })
                .ForeignKey("dbo.Creators", t => t.Creator, cascadeDelete: true)
                .ForeignKey("dbo.Works", t => t.Work, cascadeDelete: true)
                .Index(t => t.Work)
                .Index(t => t.Creator);

            CreateTable(
                "dbo.Works",
                c => new
                    {
                        Identifier = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Path = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        Rights = c.String(),
                        License = c.Int(nullable: false),
                        WorkType = c.Int(nullable: false),
                        ContentRating = c.Int(nullable: false),
                        ContentDescriptor = c.Int(nullable: false),
                        Content = c.String(),
                        DraftIdentifier = c.String(),
                        ArtifactOf_Identifier = c.Guid(),
                        CommentsOn_Identifier = c.Guid(),
                        DraftOf_Identifier = c.Guid(),
                        Language_Name = c.String(maxLength: 128),
                        Parent_Identifier = c.Guid(),
                    })
                .PrimaryKey(t => t.Identifier)
                .ForeignKey("dbo.Works", t => t.ArtifactOf_Identifier)
                .ForeignKey("dbo.Works", t => t.CommentsOn_Identifier)
                .ForeignKey("dbo.Works", t => t.DraftOf_Identifier)
                .ForeignKey("dbo.Languages", t => t.Language_Name)
                .ForeignKey("dbo.Works", t => t.Parent_Identifier)
                .Index(t => t.ArtifactOf_Identifier)
                .Index(t => t.CommentsOn_Identifier)
                .Index(t => t.DraftOf_Identifier)
                .Index(t => t.Language_Name)
                .Index(t => t.Parent_Identifier);

            CreateTable(
                "dbo.TagImplications",
                c => new
                    {
                        Tag = c.String(nullable: false, maxLength: 128),
                        Implication = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Tag, t.Implication })
                .ForeignKey("dbo.Tags", t => t.Tag)
                .ForeignKey("dbo.Tags", t => t.Implication)
                .Index(t => t.Tag)
                .Index(t => t.Implication);

            CreateTable(
                "dbo.NextWorks",
                c => new
                    {
                        Work = c.Guid(nullable: false),
                        NextWork = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Work, t.NextWork })
                .ForeignKey("dbo.Works", t => t.Work)
                .ForeignKey("dbo.Works", t => t.NextWork)
                .Index(t => t.Work)
                .Index(t => t.NextWork);

            CreateTable(
                "dbo.WorkTags",
                c => new
                    {
                        Work = c.Guid(nullable: false),
                        Tag = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Work, t.Tag })
                .ForeignKey("dbo.Works", t => t.Work, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag, cascadeDelete: true)
                .Index(t => t.Work)
                .Index(t => t.Tag);
        }

        public override void Down()
        {
            DropForeignKey("dbo.WorkCreators", "Work", "dbo.Works");
            DropForeignKey("dbo.WorkTags", "Tag", "dbo.Tags");
            DropForeignKey("dbo.WorkTags", "Work", "dbo.Works");
            DropForeignKey("dbo.Works", "Parent_Identifier", "dbo.Works");
            DropForeignKey("dbo.NextWorks", "NextWork", "dbo.Works");
            DropForeignKey("dbo.NextWorks", "Work", "dbo.Works");
            DropForeignKey("dbo.Works", "Language_Name", "dbo.Languages");
            DropForeignKey("dbo.Works", "DraftOf_Identifier", "dbo.Works");
            DropForeignKey("dbo.Works", "CommentsOn_Identifier", "dbo.Works");
            DropForeignKey("dbo.Works", "ArtifactOf_Identifier", "dbo.Works");
            DropForeignKey("dbo.WorkCreators", "Creator", "dbo.Creators");
            DropForeignKey("dbo.TagImplications", "Implication", "dbo.Tags");
            DropForeignKey("dbo.TagImplications", "Tag", "dbo.Tags");
            DropForeignKey("dbo.TagAliases", "TagName", "dbo.Tags");
            DropForeignKey("dbo.CreatorAliases", "AliasOf", "dbo.Creators");
            DropIndex("dbo.WorkTags", new[] { "Tag" });
            DropIndex("dbo.WorkTags", new[] { "Work" });
            DropIndex("dbo.NextWorks", new[] { "NextWork" });
            DropIndex("dbo.NextWorks", new[] { "Work" });
            DropIndex("dbo.TagImplications", new[] { "Implication" });
            DropIndex("dbo.TagImplications", new[] { "Tag" });
            DropIndex("dbo.Works", new[] { "Parent_Identifier" });
            DropIndex("dbo.Works", new[] { "Language_Name" });
            DropIndex("dbo.Works", new[] { "DraftOf_Identifier" });
            DropIndex("dbo.Works", new[] { "CommentsOn_Identifier" });
            DropIndex("dbo.Works", new[] { "ArtifactOf_Identifier" });
            DropIndex("dbo.WorkCreators", new[] { "Creator" });
            DropIndex("dbo.WorkCreators", new[] { "Work" });
            DropIndex("dbo.TagAliases", new[] { "TagName" });
            DropIndex("dbo.CreatorAliases", new[] { "AliasOf" });
            DropTable("dbo.WorkTags");
            DropTable("dbo.NextWorks");
            DropTable("dbo.TagImplications");
            DropTable("dbo.Works");
            DropTable("dbo.WorkCreators");
            DropTable("dbo.Tags");
            DropTable("dbo.TagAliases");
            DropTable("dbo.Languages");
            DropTable("dbo.Creators");
            DropTable("dbo.CreatorAliases");
        }
    }
}
