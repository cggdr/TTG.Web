namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaTalTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceTables", "PriceInDay_ID", c => c.Int());
            CreateIndex("dbo.PriceTables", "PriceInDay_ID");
            AddForeignKey("dbo.PriceTables", "PriceInDay_ID", "dbo.PriceInDays", "ID");
            DropColumn("dbo.Entrusts", "EntrustID");
            DropColumn("dbo.Entrusts", "SaleOrSell");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entrusts", "SaleOrSell", c => c.Int(nullable: false));
            AddColumn("dbo.Entrusts", "EntrustID", c => c.Int(nullable: false));
            DropForeignKey("dbo.PriceTables", "PriceInDay_ID", "dbo.PriceInDays");
            DropIndex("dbo.PriceTables", new[] { "PriceInDay_ID" });
            DropColumn("dbo.PriceTables", "PriceInDay_ID");
        }
    }
}
