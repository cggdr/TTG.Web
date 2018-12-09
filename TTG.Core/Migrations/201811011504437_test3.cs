namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserIdenty_ID", c => c.Int());
            CreateIndex("dbo.Users", "UserIdenty_ID");
            AddForeignKey("dbo.Users", "UserIdenty_ID", "dbo.UserIdenties", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserIdenty_ID", "dbo.UserIdenties");
            DropIndex("dbo.Users", new[] { "UserIdenty_ID" });
            DropColumn("dbo.Users", "UserIdenty_ID");
        }
    }
}
