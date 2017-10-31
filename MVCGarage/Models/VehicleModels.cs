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
        RefId,RegNr,Owner,Date,Checkout
    }

    public class Vehicle
    {
        [Key]
        public int ParkingID { get; set; }
        public VehicleType Type { get; set; }
        public string RegistrationNumber { get; set; }
        public string DateParked { get; set; } //
        public string DateCheckout { get; set; } // 
        public int ParkingSpot { get; set; }
        public int Size { get; set; }
        public string Owner { get; set; }

        public Vehicle()
        {
            DateParked = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); // With some basic formatting
            switch (Type)
            {
                case VehicleType.Car:
                case VehicleType.MC:
                    Size = 1;
                    break;
                case VehicleType.Bus:
                    Size = 3;
                    break;
                case VehicleType.Truck:
                    Size = 2;
                    break;
                default:
                    Size = 1;
                    break;
            }
        }

        public void CheckOut(string outdate = null){
            if (outdate == null)
            {
                DateCheckout = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            else 
            {
                DateCheckout = outdate;
            }
        }

        // Will this be used at all?
        public override string ToString() 
        {
            return RegistrationNumber
                + ":" + Type
                + ":" + DateParked
                + ":" + ParkingSpot
                + ":" + Size
                + ":" + Owner;
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