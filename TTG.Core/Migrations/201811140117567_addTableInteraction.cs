namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableInteraction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Interactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FWalletID = c.Int(nullable: false),
                        WalletAddress = c.String(maxLength: 256),
                        Status = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        OperatingTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Wallets", t => t.FWalletID, cascadeDelete: true)
                .Index(t => t.FWalletID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interactions", "FWalletID", "dbo.Wallets");
            DropIndex("dbo.Interactions", new[] { "FWalletID" });
            DropTable("dbo.Interactions");
        }
    }
}
