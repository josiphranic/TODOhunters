namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Texts", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Texts", "Username");
        }
    }
}
