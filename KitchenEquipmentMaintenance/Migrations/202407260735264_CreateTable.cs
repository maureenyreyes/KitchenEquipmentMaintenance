namespace KitchenEquipmentMaintenance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentId = c.Int(nullable: false, identity: true),
                        SerialNumber = c.String(),
                        Description = c.String(),
                        Condition = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
            CreateTable(
                "dbo.RegisteredEquipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EquipmentId = c.Int(nullable: false),
                        SiteId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Equipments", t => t.EquipmentId, cascadeDelete: true)
                .ForeignKey("dbo.Sites", t => t.SiteId)
                .Index(t => t.EquipmentId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SiteId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserType = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegisteredEquipments", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.RegisteredEquipments", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.RegisteredEquipments", new[] { "SiteId" });
            DropIndex("dbo.RegisteredEquipments", new[] { "EquipmentId" });
            DropTable("dbo.Users");
            DropTable("dbo.Sites");
            DropTable("dbo.RegisteredEquipments");
            DropTable("dbo.Equipments");
        }
    }
}
