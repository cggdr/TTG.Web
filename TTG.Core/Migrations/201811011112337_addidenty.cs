namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addidenty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonalIdenties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false, maxLength: 16),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Users", "PersonalIdenty_ID", c => c.Int());
            CreateIndex("dbo.Users", "PersonalIdenty_ID");
            AddForeignKey("dbo.Users", "PersonalIdenty_ID", "dbo.PersonalIdenties", "ID");
            DropColumn("dbo.VirtualCurrencies", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VirtualCurrencies", "Price", c => c.Double(nullable: false));
            DropForeignKey("dbo.Users", "PersonalIdenty_ID", "dbo.PersonalIdenties");
            DropIndex("dbo.Users", new[] { "PersonalIdenty_ID" });
            DropColumn("dbo.Users", "PersonalIdenty_ID");
            DropTable("dbo.PersonalIdenties");
        }
    }
}
