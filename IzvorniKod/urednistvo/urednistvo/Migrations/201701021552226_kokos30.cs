namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos30 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Images", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Content", c => c.Binary());
        }
    }
}
