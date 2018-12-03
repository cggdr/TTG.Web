namespace TTG.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserIdenties",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserIdenties");
        }
    }
}
