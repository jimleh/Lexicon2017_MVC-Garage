namespace MVCGarage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        ParkingID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        RegistrationNumber = c.String(),
                        Date = c.String(),
                        Spot_XPosition = c.Int(nullable: false),
                        Spot_YPosition = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        Owner = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ParkingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Vehicles");
        }
    }
}
