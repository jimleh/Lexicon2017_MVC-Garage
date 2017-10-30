using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCGarage.Models
{
    public enum VehicleType
    {
        MC, Car, Bus, Truck
    }

    public enum SearchOption 
    {
        RegNr,Owner,Date
    }

    public class ParkingSpot
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public override string ToString()
        {
            return "[" + XPosition + ", " + YPosition + "]";
        }
    }

    public class Vehicle
    {
        [Key]
        public int ParkingID { get; set; }
        public VehicleType Type { get; set; }
        public string RegistrationNumber { get; set; }
        public string Date { get; set; }
        public ParkingSpot Spot { get; set; } // Maybe change it?
        public int Size { get; set; }
        public string Owner { get; set; }

        public override string ToString()
        {
            return RegistrationNumber
                + ":" + Type
                + ":" + Date
                + ":" + Spot.ToString()
                + ":" + Size
                + ":" + Owner;
        }

        public string BasicInfo()
        {
            return RegistrationNumber + ":" + Type;
        }
    }


    public class MC : Vehicle
    {
        public MC()
        {
            Type = VehicleType.MC;
            Size = 1;
        }
    }
    public class Car : Vehicle
    {
        public Car()
        {
            Type = VehicleType.Car;
            Size = 1;
        }
    }
    public class Bus : Vehicle
    {
        public Bus()
        {
            Type = VehicleType.Bus;
            Size = 3;
        }
    }
    public class Truck : Vehicle
    {
        public Truck()
        {
            Type = VehicleType.Truck;
            Size = 3;
        }
    }
}