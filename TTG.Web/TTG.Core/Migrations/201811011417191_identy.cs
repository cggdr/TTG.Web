namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class identy : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PersonalIdenties", newName: "Personals");
            RenameColumn(table: "dbo.Users", name: "PersonalIdenty_ID", newName: "Personal_ID");
            RenameIndex(table: "dbo.Users", name: "IX_PersonalIdenty_ID", newName: "IX_Personal_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "IX_Personal_ID", newName: "IX_PersonalIdenty_ID");
            RenameColumn(table: "dbo.Users", name: "Personal_ID", newName: "PersonalIdenty_ID");
            RenameTable(name: "dbo.Personals", newName: "PersonalIdenties");
        }
    }
}
