namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Texts", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Texts", "Time");
        }
    }
}
