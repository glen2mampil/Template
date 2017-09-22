namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedPermissionModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Permissions", "ModuleId", c => c.Int(nullable: true));
            CreateIndex("dbo.Permissions", "ModuleId");
            AddForeignKey("dbo.Permissions", "ModuleId", "dbo.MasterDetails", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "ModuleId", "dbo.MasterDetails");
            DropIndex("dbo.Permissions", new[] { "ModuleId" });
            DropColumn("dbo.Permissions", "ModuleId");
        }
    }
}
