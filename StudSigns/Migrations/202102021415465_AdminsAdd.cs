namespace StudSigns.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminsAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        Pass = c.String(nullable: false),
                        isSuperAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Login);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Administrators");
        }
    }
}
