namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "Type");
        }
    }
}
