namespace urednistvo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kokos25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        RateId = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RateId);
            
            AddColumn("dbo.Roles", "Value", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "Value");
            DropTable("dbo.Rates");
        }
    }
}
