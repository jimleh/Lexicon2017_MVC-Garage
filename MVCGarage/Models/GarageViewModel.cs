using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGarage.Models
{
    public class GarageViewModel
    {
        public bool[,,] parkingSpots;
        public List<Vehicle> vehicles = new List<Vehicle>();
    }
}