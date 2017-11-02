namespace MVCGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParkingSpotModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkingSpots",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Occupied = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParkingSpots");
        }
    }
}
