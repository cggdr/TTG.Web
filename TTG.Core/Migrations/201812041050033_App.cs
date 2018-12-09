namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class App : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "FileAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "FileAddress");
        }
    }
}
