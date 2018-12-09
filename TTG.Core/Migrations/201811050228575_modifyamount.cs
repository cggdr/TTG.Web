namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyamount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entrusts", "Amount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entrusts", "Amount", c => c.Int(nullable: false));
        }
    }
}
