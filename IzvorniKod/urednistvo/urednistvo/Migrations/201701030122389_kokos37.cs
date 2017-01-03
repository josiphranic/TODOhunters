namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos37 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Rrfs", "TextId", "dbo.Texts");
            DropIndex("dbo.Rrfs", new[] { "TextId" });
            CreateTable(
                "dbo.Pdfs",
                c => new
                    {
                        PdfId = c.Int(nullable: false, identity: true),
                        PdfName = c.String(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PdfId)
                .ForeignKey("dbo.Texts", t => t.TextId, cascadeDelete: true)
                .Index(t => t.TextId);
            
            DropTable("dbo.Rrfs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Rrfs",
                c => new
                    {
                        RtfId = c.Int(nullable: false, identity: true),
                        RtfName = c.String(),
                        TextId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RtfId);
            
            DropForeignKey("dbo.Pdfs", "TextId", "dbo.Texts");
            DropIndex("dbo.Pdfs", new[] { "TextId" });
            DropTable("dbo.Pdfs");
            CreateIndex("dbo.Rrfs", "TextId");
            AddForeignKey("dbo.Rrfs", "TextId", "dbo.Texts", "TextId", cascadeDelete: true);
        }
    }
}
