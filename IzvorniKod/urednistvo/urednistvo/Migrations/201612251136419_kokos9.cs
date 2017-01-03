namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Texts", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Texts", "Username", c => c.String());
        }
    }
}
