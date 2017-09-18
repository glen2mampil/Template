namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMasterDetailNavigationInAuditTrail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuditTrails", "MasterDetails_Id", "dbo.MasterDetails");
            DropIndex("dbo.AuditTrails", new[] { "MasterDetails_Id" });
            DropColumn("dbo.AuditTrails", "ModuleId");
            RenameColumn(table: "dbo.AuditTrails", name: "MasterDetails_Id", newName: "ModuleId");
            AlterColumn("dbo.AuditTrails", "ModuleId", c => c.Int(nullable: true));
            CreateIndex("dbo.AuditTrails", "ModuleId");
            AddForeignKey("dbo.AuditTrails", "ModuleId", "dbo.MasterDetails", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditTrails", "ModuleId", "dbo.MasterDetails");
            DropIndex("dbo.AuditTrails", new[] { "ModuleId" });
            AlterColumn("dbo.AuditTrails", "ModuleId", c => c.Int());
            RenameColumn(table: "dbo.AuditTrails", name: "ModuleId", newName: "MasterDetails_Id");
            AddColumn("dbo.AuditTrails", "ModuleId", c => c.Int(nullable: false));
            CreateIndex("dbo.AuditTrails", "MasterDetails_Id");
            AddForeignKey("dbo.AuditTrails", "MasterDetails_Id", "dbo.MasterDetails", "Id");
        }
    }
}
