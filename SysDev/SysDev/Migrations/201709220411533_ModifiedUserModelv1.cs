namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedUserModelv1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "RoleId", "dbo.MasterDetails");
            DropIndex("dbo.AspNetUsers", new[] { "RoleId" });
            AddColumn("dbo.AspNetUsers", "PermissionId", c => c.Int(nullable: true));
            CreateIndex("dbo.AspNetUsers", "PermissionId");
            AddForeignKey("dbo.AspNetUsers", "PermissionId", "dbo.Permissions", "Id", cascadeDelete: false);
            DropColumn("dbo.AspNetUsers", "RoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RoleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AspNetUsers", "PermissionId", "dbo.Permissions");
            DropIndex("dbo.AspNetUsers", new[] { "PermissionId" });
            DropColumn("dbo.AspNetUsers", "PermissionId");
            CreateIndex("dbo.AspNetUsers", "RoleId");
            AddForeignKey("dbo.AspNetUsers", "RoleId", "dbo.MasterDetails", "Id", cascadeDelete: true);
        }
    }
}
