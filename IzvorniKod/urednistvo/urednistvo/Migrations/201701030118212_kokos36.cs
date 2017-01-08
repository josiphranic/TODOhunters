namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos36 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pdfs", "TextId", "dbo.Texts");
            DropIndex("dbo.Pdfs", new[] { "TextId" });
            CreateTable(
                "dbo.Rrfs",
                c => new
                    {
                        RtfId = c.Int(nullable: false, identity: true),
                        RtfName = c.String(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RtfId)
                .ForeignKey("dbo.Texts", t => t.TextId, cascadeDelete: true)
                .Index(t => t.TextId);
            
            DropTable("dbo.Pdfs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pdfs",
                c => new
                    {
                        PdfId = c.Int(nullable: false, identity: true),
                        PdfName = c.String(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PdfId);
            
            DropForeignKey("dbo.Rrfs", "TextId", "dbo.Texts");
            DropIndex("dbo.Rrfs", new[] { "TextId" });
            DropTable("dbo.Rrfs");
            CreateIndex("dbo.Pdfs", "TextId");
            AddForeignKey("dbo.Pdfs", "TextId", "dbo.Texts", "TextId", cascadeDelete: true);
        }
    }
}
