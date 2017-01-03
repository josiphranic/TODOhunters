namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos171 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Texts", "Public", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Texts", "Public", c => c.Boolean(nullable: false));
        }
    }
}
