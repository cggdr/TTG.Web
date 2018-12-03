namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifypassword : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PersonalIdenties", "Password", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonalIdenties", "Password", c => c.String(nullable: false, maxLength: 16));
        }
    }
}
