namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokot3 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Texts", t => t.TextId, cascadeDelete: true)
                .Index(t => t.TextId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "TextId", "dbo.Texts");
            DropIndex("dbo.Images", new[] { "TextId" });
            DropTable("dbo.Images");
        }
    }
}
