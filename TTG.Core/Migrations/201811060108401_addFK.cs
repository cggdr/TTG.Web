namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EntrustDetails", "Entrust_ID", "dbo.Entrusts");
            DropForeignKey("dbo.PriceInADeals", "PriceInDay_ID", "dbo.PriceInDays");
            DropIndex("dbo.EntrustDetails", new[] { "Entrust_ID" });
            DropIndex("dbo.PriceInADeals", new[] { "PriceInDay_ID" });
            RenameColumn(table: "dbo.EntrustDetails", name: "Entrust_ID", newName: "EntrustID");
            RenameColumn(table: "dbo.PriceInADeals", name: "PriceInDay_ID", newName: "PriceInDayID");
            AlterColumn("dbo.EntrustDetails", "EntrustID", c => c.Int(nullable: false));
            AlterColumn("dbo.PriceInADeals", "PriceInDayID", c => c.Int(nullable: false));
            CreateIndex("dbo.EntrustDetails", "EntrustID");
            CreateIndex("dbo.PriceInADeals", "PriceInDayID");
            AddForeignKey("dbo.EntrustDetails", "EntrustID", "dbo.Entrusts", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PriceInADeals", "PriceInDayID", "dbo.PriceInDays", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceInADeals", "PriceInDayID", "dbo.PriceInDays");
            DropForeignKey("dbo.EntrustDetails", "EntrustID", "dbo.Entrusts");
            DropIndex("dbo.PriceInADeals", new[] { "PriceInDayID" });
            DropIndex("dbo.EntrustDetails", new[] { "EntrustID" });
            AlterColumn("dbo.PriceInADeals", "PriceInDayID", c => c.Int());
            AlterColumn("dbo.EntrustDetails", "EntrustID", c => c.Int());
            RenameColumn(table: "dbo.PriceInADeals", name: "PriceInDayID", newName: "PriceInDay_ID");
            RenameColumn(table: "dbo.EntrustDetails", name: "EntrustID", newName: "Entrust_ID");
            CreateIndex("dbo.PriceInADeals", "PriceInDay_ID");
            CreateIndex("dbo.EntrustDetails", "Entrust_ID");
            AddForeignKey("dbo.PriceInADeals", "PriceInDay_ID", "dbo.PriceInDays", "ID");
            AddForeignKey("dbo.EntrustDetails", "Entrust_ID", "dbo.Entrusts", "ID");
        }
    }
}
