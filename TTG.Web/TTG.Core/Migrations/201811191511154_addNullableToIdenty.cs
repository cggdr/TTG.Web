namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNullableToIdenty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserIdenties", "Password", c => c.String(nullable: true, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserIdenties", "Password", c => c.String(nullable: true, maxLength: 256));
        }
    }
}
