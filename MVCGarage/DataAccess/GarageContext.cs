using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCGarage
{
    public class GarageContext : DbContext
    {
        public DbSet<Models.Vehicle> Vehicles { get; set; }

        public GarageContext() : base("DefaultConnection") 
        { 
        
        }

    }
}