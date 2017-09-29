namespace SysDev.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedUserModelv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Permission_Id", "dbo.Permissions");
            DropIndex("dbo.AspNetUsers", new[] { "Permission_Id" });
            DropColumn("dbo.AspNetUsers", "Permission_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Permission_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Permission_Id");
            AddForeignKey("dbo.AspNetUsers", "Permission_Id", "dbo.Permissions", "Id");
        }
    }
}
