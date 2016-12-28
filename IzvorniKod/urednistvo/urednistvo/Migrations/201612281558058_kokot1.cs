namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokot1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "TextId", "dbo.Texts");
            DropIndex("dbo.Images", new[] { "TextId" });
            DropPrimaryKey("dbo.Comments");
            AddColumn("dbo.Comments", "CommentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Comments", "CommentId");
            DropTable("dbo.Images");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        Content = c.Binary(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageId);
            
            DropPrimaryKey("dbo.Comments");
            DropColumn("dbo.Comments", "CommentId");
            AddPrimaryKey("dbo.Comments", new[] { "UserId", "TextId" });
            CreateIndex("dbo.Images", "TextId");
            AddForeignKey("dbo.Images", "TextId", "dbo.Texts", "TextId", cascadeDelete: true);
        }
    }
}
