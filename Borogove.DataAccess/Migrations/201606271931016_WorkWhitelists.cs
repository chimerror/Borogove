using System.Data.Entity.Migrations;

namespace Borogove.DataAccess.Migrations
{
    public partial class WorkWhitelists : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Security.WorkWhitelists",
                c => new
                {
                    Work = c.Guid(nullable: false),
                    SubjectType = c.Int(nullable: false),
                    SubjectName = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => new { t.Work, t.SubjectType, t.SubjectName })
                .ForeignKey("dbo.Works", t => t.Work, cascadeDelete: true)
                .Index(t => t.Work);

            Sql(@"
CREATE FUNCTION Security.WorkWhitelistPredicate(@WorkIdentifier AS uniqueidentifier)
    RETURNS TABLE
    WITH SCHEMABINDING
AS
    RETURN SELECT TOP 1 1 AS accessResult
    FROM Security.WorkWhitelists AS ww1
    WHERE
        (CAST(SESSION_CONTEXT(N'Groups') AS nvarchar(255)) LIKE N'%|Administrators|%') OR
        (ww1.Work = @WorkIdentifier AND
         ww1.SubjectType = 0 AND
         ww1.SubjectName = CAST(SESSION_CONTEXT(N'UserId') AS nvarchar(255))) OR
        (ww1.Work = @WorkIdentifier AND
         ww1.SubjectType = 1 AND
         CAST(SESSION_CONTEXT(N'Groups') AS nvarchar(255)) LIKE N'%|' + ww1.SubjectName + N'|%')
     UNION ALL
     SELECT 1 AS accessResult
     WHERE NOT EXISTS (SELECT 1 FROM Security.WorkWhitelists AS ww2 WHERE ww2.Work = @WorkIdentifier)
GO

CREATE SECURITY POLICY Security.WorksSecurityPolicy
    ADD FILTER PREDICATE Security.WorkWhitelistPredicate(Identifier) ON dbo.Works;
GO");
        }

        public override void Down()
        {
            Sql(@"
DROP SECURITY POLICY Security.WorksSecurityPolicy;
GO

DROP FUNCTION Security.WorkWhitelistPredicate;
GO");

            DropForeignKey("Security.WorkWhitelists", "Work", "dbo.Works");
            DropIndex("Security.WorkWhitelists", new[] { "Work" });
            DropTable("Security.WorkWhitelists");
        }
    }
}
