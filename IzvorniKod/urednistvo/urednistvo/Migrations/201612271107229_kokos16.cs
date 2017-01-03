namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sections", "Edition_EditionId", "dbo.Editions");
            DropIndex("dbo.Sections", new[] { "Edition_EditionId" });
            RenameColumn(table: "dbo.Sections", name: "Edition_EditionId", newName: "EditionId");
            AlterColumn("dbo.Sections", "EditionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sections", "EditionId");
            AddForeignKey("dbo.Sections", "EditionId", "dbo.Editions", "EditionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "EditionId", "dbo.Editions");
            DropIndex("dbo.Sections", new[] { "EditionId" });
            AlterColumn("dbo.Sections", "EditionId", c => c.Int());
            RenameColumn(table: "dbo.Sections", name: "EditionId", newName: "Edition_EditionId");
            CreateIndex("dbo.Sections", "Edition_EditionId");
            AddForeignKey("dbo.Sections", "Edition_EditionId", "dbo.Editions", "EditionId");
        }
    }
}
