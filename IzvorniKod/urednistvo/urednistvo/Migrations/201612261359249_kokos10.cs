namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos10 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Texts", "Suggestions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Texts", "Suggestions", c => c.String());
        }
    }
}
