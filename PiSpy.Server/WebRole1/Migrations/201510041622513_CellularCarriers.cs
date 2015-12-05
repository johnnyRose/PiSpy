namespace WebRole1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CellularCarriers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CellularCarriers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EmailExtension = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "CellularCarrierId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "CellularCarrierId");
            AddForeignKey("dbo.AspNetUsers", "CellularCarrierId", "dbo.CellularCarriers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CellularCarrierId", "dbo.CellularCarriers");
            DropIndex("dbo.AspNetUsers", new[] { "CellularCarrierId" });
            DropColumn("dbo.AspNetUsers", "CellularCarrierId");
            DropTable("dbo.CellularCarriers");
        }
    }
}
