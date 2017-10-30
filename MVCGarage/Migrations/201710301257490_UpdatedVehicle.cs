namespace MVCGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedVehicle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "DateParked", c => c.String());
            AddColumn("dbo.Vehicles", "DateCheckout", c => c.String());
            AddColumn("dbo.Vehicles", "ParkingSpot", c => c.Int(nullable: false));
            DropColumn("dbo.Vehicles", "Date");
            DropColumn("dbo.Vehicles", "Spot_XPosition");
            DropColumn("dbo.Vehicles", "Spot_YPosition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Spot_YPosition", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "Spot_XPosition", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "Date", c => c.String());
            DropColumn("dbo.Vehicles", "ParkingSpot");
            DropColumn("dbo.Vehicles", "DateCheckout");
            DropColumn("dbo.Vehicles", "DateParked");
        }
    }
}
