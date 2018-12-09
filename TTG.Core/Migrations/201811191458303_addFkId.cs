namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFkId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserIdenty_ID", "dbo.UserIdenties");
            DropIndex("dbo.Users", new[] { "UserIdenty_ID" });
            RenameColumn(table: "dbo.Users", name: "UserIdenty_ID", newName: "FKIdentyID");
            AlterColumn("dbo.Users", "FKIdentyID", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "FKIdentyID");
            AddForeignKey("dbo.Users", "FKIdentyID", "dbo.UserIdenties", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "FKIdentyID", "dbo.UserIdenties");
            DropIndex("dbo.Users", new[] { "FKIdentyID" });
            AlterColumn("dbo.Users", "FKIdentyID", c => c.Int());
            RenameColumn(table: "dbo.Users", name: "FKIdentyID", newName: "UserIdenty_ID");
            CreateIndex("dbo.Users", "UserIdenty_ID");
            AddForeignKey("dbo.Users", "UserIdenty_ID", "dbo.UserIdenties", "ID");
        }
    }
}
