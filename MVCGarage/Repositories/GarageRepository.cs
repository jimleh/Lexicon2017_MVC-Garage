using MVCGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCGarage.Repositories
{
    public class GarageRepository
    {
        private GarageContext context;

        public GarageRepository()
        {
            context = new GarageContext();
        }

        public IEnumerable<Vehicle> getAllVehicles()
        {
            return context.Vehicles.ToList();
        }

        public IEnumerable<Vehicle> getFilteredVehicles()
        {
            return context.Vehicles.ToList();
        
        }


    }
}