namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Texts", "Suggestions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Texts", "Suggestions");
        }
    }
}
