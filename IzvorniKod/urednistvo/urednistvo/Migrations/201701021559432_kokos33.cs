namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos33 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pdfs", "TextId", "dbo.Texts");
            DropIndex("dbo.Pdfs", new[] { "TextId" });
            DropTable("dbo.Pdfs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pdfs",
                c => new
                    {
                        PfdId = c.Int(nullable: false, identity: true),
                        PdfName = c.String(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PfdId);
            
            CreateIndex("dbo.Pdfs", "TextId");
            AddForeignKey("dbo.Pdfs", "TextId", "dbo.Texts", "TextId", cascadeDelete: true);
        }
    }
}
