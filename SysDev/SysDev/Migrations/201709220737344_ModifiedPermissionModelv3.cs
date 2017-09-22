namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedPermissionModelv3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Permissions", name: "MasterDetailId", newName: "ModuleId");
            RenameIndex(table: "dbo.Permissions", name: "IX_MasterDetailId", newName: "IX_ModuleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Permissions", name: "IX_ModuleId", newName: "IX_MasterDetailId");
            RenameColumn(table: "dbo.Permissions", name: "ModuleId", newName: "MasterDetailId");
        }
    }
}
