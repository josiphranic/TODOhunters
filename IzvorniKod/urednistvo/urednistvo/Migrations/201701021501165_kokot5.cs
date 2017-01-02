namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokot5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Editions", "Title", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Editions", "Title", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
