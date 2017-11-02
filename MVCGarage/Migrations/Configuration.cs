namespace MVCGarage.Migrations
{
    using MVCGarage.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCGarage.GarageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCGarage.GarageContext context)
        {
            context.Vehicles.AddOrUpdate(
                    v => v.ParkingID,
                    new Vehicle
                    {
                        ParkingID = 1,
                        Type = VehicleType.Car,
                        RegistrationNumber = "ABC-123",
                        DateParked = DateTime.Now.ToString(),
                        Size = 3,
                        ParkingSpot = 1,
                        Owner = "Christine";

                    }
                );
        }
    }
}