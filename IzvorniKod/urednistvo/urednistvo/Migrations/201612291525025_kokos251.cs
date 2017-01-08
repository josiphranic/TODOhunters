namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos251 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rates", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rates", "Name", c => c.Int(nullable: false));
        }
    }
}
