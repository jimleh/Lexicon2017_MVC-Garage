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
        private SearchOption currentSort;
        private bool sortAscending;

        public GarageRepository()
        {
            context = new GarageContext();
            currentSort = SearchOption.RefId;
            sortAscending = false;
        }

        public IEnumerable<Vehicle> getAllVehicles()
        {
            return context.Vehicles.ToList();
        }

        public Vehicle getSpecificVehicle(int id) 
        {
            Vehicle v = context.Vehicles.FirstOrDefault(a => a.ParkingID == id);
            return v;
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
                        case (int)SearchOption.RefId:
                            result = result.Union(
                                context.Vehicles.Where(
                                    a => a.ParkingID.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
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
                                    a => a.DateParked.ToString().ToLower().Contains(
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

        public IEnumerable<Vehicle> SortlistBy(IEnumerable<Vehicle> query,SearchOption s) {

            if (s == currentSort){
                        sortAscending = !sortAscending;
                    }else{
                        currentSort = s;
                        sortAscending = true;
                    }

            Func<Vehicle, object> predicate = null;

            switch (s)
            {
                case SearchOption.RefId:
                    predicate = a => a.ParkingID;
                    break;
                case SearchOption.RegNr:
                    predicate = a => a.RegistrationNumber;
                    break;
                case SearchOption.Owner:
                    predicate = a => a.Owner;
                    break;
                case SearchOption.Date:
                    predicate = a => a.DateParked;
                    break;
            }

            if (sortAscending)
            {
                query = query.OrderBy(predicate);
            }
            else
            {
                query = query.OrderByDescending(predicate);
            }

            return (query.ToList());

        }




    }
}