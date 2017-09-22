namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedPermissionModelv1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Permissions", name: "ModuleId", newName: "RoleId");
            RenameIndex(table: "dbo.Permissions", name: "IX_ModuleId", newName: "IX_RoleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Permissions", name: "IX_RoleId", newName: "IX_ModuleId");
            RenameColumn(table: "dbo.Permissions", name: "RoleId", newName: "ModuleId");
        }
    }
}
