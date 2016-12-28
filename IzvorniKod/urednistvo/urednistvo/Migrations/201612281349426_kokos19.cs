namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos19 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Texts", "Public");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Texts", "Public", c => c.Boolean());
        }
    }
}
