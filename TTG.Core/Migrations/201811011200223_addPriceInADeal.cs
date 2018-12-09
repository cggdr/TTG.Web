namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPriceInADeal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceInADeals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DealTime = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        PriceInDay_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PriceInDays", t => t.PriceInDay_ID)
                .Index(t => t.PriceInDay_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceInADeals", "PriceInDay_ID", "dbo.PriceInDays");
            DropIndex("dbo.PriceInADeals", new[] { "PriceInDay_ID" });
            DropTable("dbo.PriceInADeals");
        }
    }
}
