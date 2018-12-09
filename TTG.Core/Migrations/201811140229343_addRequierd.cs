namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequierd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interactions", "InOrOut", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interactions", "InOrOut");
        }
    }
}
