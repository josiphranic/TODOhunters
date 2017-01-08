namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos31 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pdfs",
                c => new
                    {
                        PfdId = c.Int(nullable: false, identity: true),
                        PdfName = c.String(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PfdId)
                .ForeignKey("dbo.Texts", t => t.TextId, cascadeDelete: true)
                .Index(t => t.TextId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pdfs", "TextId", "dbo.Texts");
            DropIndex("dbo.Pdfs", new[] { "TextId" });
            DropTable("dbo.Pdfs");
        }
    }
}
