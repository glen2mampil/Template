namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        PageId = c.Int(nullable: false),
                        Action = c.String(),
                        UserProfileId = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 50),
                        ContactNo = c.String(nullable: false, maxLength: 50),
                        CompanyName = c.String(nullable: false, maxLength: 50),
                        CompanyId = c.String(nullable: false, maxLength: 50),
                        Gender = c.String(nullable: false, maxLength: 50),
                        MaritalStatus = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MasterDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.String(),
                        OrderNumber = c.Int(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                        DateTimeUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MasterDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        Status = c.String(),
                        OrderNumber = c.Int(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                        DateTimeUpdated = c.DateTime(nullable: false),
                        MasterDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MasterDatas", t => t.MasterDataId, cascadeDelete: true)
                .Index(t => t.MasterDataId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdentityRoleId = c.String(maxLength: 128),
                        MasterDetailId = c.Int(nullable: false),
                        AllowView = c.Int(nullable: false),
                        AllowCreate = c.Int(nullable: false),
                        AllowEdit = c.Int(nullable: false),
                        AllowDelete = c.Int(nullable: false),
                        AllowGenerateReport = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.IdentityRoleId)
                .ForeignKey("dbo.MasterDetails", t => t.MasterDetailId, cascadeDelete: true)
                .Index(t => t.IdentityRoleId)
                .Index(t => t.MasterDetailId);
            
            AddColumn("dbo.AspNetUsers", "Status", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserProfileId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "UserProfileId");
            AddForeignKey("dbo.AspNetUsers", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.Permissions", "MasterDetailId", "dbo.MasterDetails");
            DropForeignKey("dbo.Permissions", "IdentityRoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.MasterDetails", "MasterDataId", "dbo.MasterDatas");
            DropForeignKey("dbo.AuditTrails", "UserProfileId", "dbo.UserProfiles");
            DropIndex("dbo.AspNetUsers", new[] { "UserProfileId" });
            DropIndex("dbo.Permissions", new[] { "MasterDetailId" });
            DropIndex("dbo.Permissions", new[] { "IdentityRoleId" });
            DropIndex("dbo.MasterDetails", new[] { "MasterDataId" });
            DropIndex("dbo.AuditTrails", new[] { "UserProfileId" });
            DropColumn("dbo.AspNetUsers", "UserProfileId");
            DropColumn("dbo.AspNetUsers", "Status");
            DropTable("dbo.Permissions");
            DropTable("dbo.MasterDetails");
            DropTable("dbo.MasterDatas");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.AuditTrails");
        }
    }
}
