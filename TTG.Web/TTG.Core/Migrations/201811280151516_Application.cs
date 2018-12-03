namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Application : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CoinName = c.String(nullable: false, maxLength: 10),
                        ApplicantName = c.String(nullable: false, maxLength: 8),
                        PhoneNum = c.String(nullable: false, maxLength: 12),
                        Email = c.String(nullable: false, maxLength: 50),
                        CompanyName = c.String(nullable: false, maxLength: 36),
                        CreateTime = c.DateTime(nullable: false),
                        ReviewTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Applications");
        }
    }
}
