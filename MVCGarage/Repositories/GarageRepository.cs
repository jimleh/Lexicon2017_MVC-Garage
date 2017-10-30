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

        public IEnumerable<Vehicle> getFilteredVehicles(string search, bool[] options)
        {
            IEnumerable<Vehicle> result = new List<Vehicle>();

            for (int i = 0; i < options.Length; i++)
            {
                if (options[i])
                {
                    switch (i)
                    {
                        case (int)SearchOption.RegNr:
                            result = result.Union(
                                context.Vehicles.Where(
                                    a => a.RegistrationNumber.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
                        case (int)SearchOption.Owner:
                            result = result.Union(
                                context.Vehicles.Where(
                                    a => a.Owner.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
                        case (int)SearchOption.Date:
                            result = result.Union(
                                context.Vehicles.Where(
                                    a => a.Date.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
                    }
                }
            }
            return result;
        }




    }
}