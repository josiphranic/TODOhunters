namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Texts", "Public", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Texts", "Public");
        }
    }
}
