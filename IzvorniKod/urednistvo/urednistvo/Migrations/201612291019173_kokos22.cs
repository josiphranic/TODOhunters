namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Editions", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Editions", "Title");
        }
    }
}
