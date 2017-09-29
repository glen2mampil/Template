namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RoleId", c => c.Int(nullable: true));
            CreateIndex("dbo.AspNetUsers", "RoleId");
            AddForeignKey("dbo.AspNetUsers", "RoleId", "dbo.MasterDetails", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "RoleId", "dbo.MasterDetails");
            DropIndex("dbo.AspNetUsers", new[] { "RoleId" });
            DropColumn("dbo.AspNetUsers", "RoleId");
        }
    }
}
