namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokot2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sections", "EditionId", "dbo.Editions");
            DropIndex("dbo.Sections", new[] { "EditionId" });
            RenameColumn(table: "dbo.Sections", name: "EditionId", newName: "Edition_EditionId");
            AlterColumn("dbo.Sections", "Edition_EditionId", c => c.Int());
            CreateIndex("dbo.Sections", "Edition_EditionId");
            AddForeignKey("dbo.Sections", "Edition_EditionId", "dbo.Editions", "EditionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sections", "Edition_EditionId", "dbo.Editions");
            DropIndex("dbo.Sections", new[] { "Edition_EditionId" });
            AlterColumn("dbo.Sections", "Edition_EditionId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Sections", name: "Edition_EditionId", newName: "EditionId");
            CreateIndex("dbo.Sections", "EditionId");
            AddForeignKey("dbo.Sections", "EditionId", "dbo.Editions", "EditionId", cascadeDelete: true);
        }
    }
}
