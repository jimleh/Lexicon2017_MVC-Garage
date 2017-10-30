using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGarage.Models
{
    public class Vehicle
    {
        public string Type { get; set; }
        public string RegistrationNumber { get; set; }
        public string Date { get; set; }
        public string Spot { get; set; }
        public int Size { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }


    public class MC : Vehicle
    {
        public MC()
        {
            Type = "MC";
            Size = 1;
        }
    }
    public class Car : Vehicle
    {
        public Car()
        {
            Type = "Car";
            Size = 1;
        }
    }
    public class BUS : Vehicle
    {
        public BUS()
        {
            Type = "BUS";
            Size = 3;
        }
    }
    public class Truck : Vehicle
    {
        public Truck()
        {
            Type = "Truck";
            Size = 3;
        }
    }
}