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
            for (int i = 1; i <= 10; i++)
            {
                context.Vehicles.AddOrUpdate(
                    v => v.ParkingID,
                    new Vehicle
                    {
                        ParkingID = i,
                        Type = VehicleType.Car,
                        RegistrationNumber = "111-" + i.ToString() + i.ToString() + i.ToString(),
                        DateParked = DateTime.Now.ToString(),
                        ParkingSpot = i,
                        Size = 1,
                        Owner = "Christine" + i.ToString()

                    }
                );
            }

            int index = 1;
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 25; j++)
                {
                    // Meh
                    bool occupied;
                    if (context.Vehicles.FirstOrDefault(v => v.ParkingSpot == index) != null)
                    {
                        occupied = true;
                    }
                    else
                    {
                        occupied = false;
                    }
                    context.ParkingSpots.AddOrUpdate(
                        s => s.ID,
                        new ParkingSpot { ID = index++, X = i, Y = j, Occupied = occupied }
                        );
                }
            }
        }
    }
}