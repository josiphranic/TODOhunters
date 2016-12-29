namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "SectionId", c => c.Int(nullable: false));
            DropColumn("dbo.Ratings", "SectionTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "SectionTitle", c => c.String());
            DropColumn("dbo.Ratings", "SectionId");
        }
    }
}
