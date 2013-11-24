namespace NowOnline.AppHarbor.Repositories
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTagEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "ApplicationId", "dbo.Applications");
            DropIndex("dbo.Tags", new[] { "ApplicationId" });
            DropColumn("dbo.Applications", "BitBucketName");
            DropTable("dbo.Tags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Commit = c.String(),
                        Created = c.DateTime(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Applications", "BitBucketName", c => c.String());
            CreateIndex("dbo.Tags", "ApplicationId");
            AddForeignKey("dbo.Tags", "ApplicationId", "dbo.Applications", "Id", cascadeDelete: true);
        }
    }
}
