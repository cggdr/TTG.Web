namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Personal_ID", "dbo.Personals");
            DropIndex("dbo.Users", new[] { "Personal_ID" });
            DropColumn("dbo.Users", "Personal_ID");
      
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Personals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Users", "Personal_ID", c => c.Int());
            CreateIndex("dbo.Users", "Personal_ID");
            AddForeignKey("dbo.Users", "Personal_ID", "dbo.Personals", "ID");
        }
    }
}
