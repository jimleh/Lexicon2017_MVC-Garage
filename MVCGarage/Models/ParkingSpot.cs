using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCGarage.Models
{
    public class ParkingSpot
    {
        [Key]
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Occupied { get; set; }
    }
}