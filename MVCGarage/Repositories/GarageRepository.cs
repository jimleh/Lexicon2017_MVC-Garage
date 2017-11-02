using MVCGarage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCGarage.Repositories
{
    public class GarageRepository
    {
        private GarageContext context;
        private SearchOption currentSort;
        private bool sortAscending;

        //private bool[] parkingspots = new bool[100];
        private bool[,] parkingSpots = new bool[10, 25];

        public GarageRepository()
        {
            context = new GarageContext();
            currentSort = SearchOption.RefId;
            sortAscending = false;
            InitParkingSpots();
        }

        // Find all the occupied parking slots
        protected void InitParkingSpots()
        {
            int index = 0;
            for (int i = 0; i < parkingSpots.GetLength(0); i++)
            {
                for (int j = 0; j < parkingSpots.GetLength(1); j++)
                {
                    index++;
                    var tmp = context.Vehicles.FirstOrDefault(v => v.ParkingSpot == index);
                    if (tmp != null)
                    {
                        for(int k = 0; k < tmp.Size; k++)
                        {
                            parkingSpots[i, j + k] = true;
                        }
                    }
                }
            }
        }

        public IEnumerable<Vehicle> getAllVehicles()
        {
            return context.Vehicles.ToList();
        }

        public Vehicle getSpecificVehicle(int? id)
        {
            Vehicle v = context.Vehicles.FirstOrDefault(a => a.ParkingID == id);
            return v;
        }

        public IEnumerable<Vehicle> getFilteredVehicles(string search = null, bool[] options = null)
        {
            IEnumerable<Vehicle> result = new List<Vehicle>();
            IEnumerable<Vehicle> query = context.Vehicles;

            if (!options[(int)SearchOption.Checkout])
            {
                query = query.Where(a => a.DateCheckout == null);
            }

            if (search == null)
            {
                result = query;
            }


            for (int i = 0; i < options.Length; i++)
            {
                if (options[i])
                {
                    switch (i)
                    {
                        case (int)SearchOption.RefId:
                            result = result.Union(
                                query.Where(
                                    a => a.ParkingID.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
                        case (int)SearchOption.RegNr:
                            result = result.Union(
                                query.Where(
                                    a => a.RegistrationNumber.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
                        case (int)SearchOption.Owner:
                            result = result.Union(
                                query.Where(
                                    a => a.Owner.ToString().ToLower().Contains(
                                        search.ToLower()
                                    )
                                )
                            );
                            break;
                        case (int)SearchOption.Date:
                            result = result.Union(
                                query.Where(
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

        public IEnumerable<Vehicle> SortlistBy(IEnumerable<Vehicle> query, SearchOption s)
        {

            if (s == currentSort)
            {
                sortAscending = !sortAscending;
            }
            else
            {
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

        public void CheckInVehicle(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            // Is this supposed to be here?
            ClearParkingSpotsForVehicle(vehicle.ParkingID, vehicle.Size);
            context.Vehicles.Remove(vehicle);
            context.SaveChanges();
        }

        public void EditVehicle(Vehicle vehicle)
        {
            context.Entry(vehicle).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void CheckOutVehicle(Vehicle vehicle)
        {
            vehicle.CheckOut();

            context.Entry(vehicle).State = EntityState.Modified;
            context.SaveChanges();
        }

        // To clear all the parking spots after a vehicle has been removed
        protected void ClearParkingSpotsForVehicle(int id, int size)
        {
            int index = 0;
            for (int i = 0; i < parkingSpots.GetLength(0); i++)
            {
                for (int j = 0; j < parkingSpots.GetLength(1); j++)
                {
                    index++;
                    if(index == id)
                    {
                        for (int k = 0; k < size; k++)
                        {
                            parkingSpots[i, j + k] = false;
                        }
                    }
                }
            }
        }

        // Modified version of the previous ParkingSpotCheckFree method
        private bool ParkingSpotCheckFree(int x, int start, int size)
        {
            for (int i = start; i < start + size; i++)
            {
                if (i > parkingSpots.GetLength(1) || parkingSpots[x, i])
                {
                    return false;
                }
            }
            return true;
        }

        // 2d, but with an array instad of using the database
        public int GetParkingSpot(int size)
        {
            int index = 1;

            for(int i = 0; i < parkingSpots.GetLength(0); i++)
            {
                for(int j = 0; j < parkingSpots.GetLength(1); j++)
                {
                    if(ParkingSpotCheckFree(i, j, size))
                    {
                        for (int k = 0; k < size; k++)
                        {
                            parkingSpots[i, j + k] = true;
                        }
                        return index;
                    }
                    index++;
                }
            }
            return -1;
        }


        //private int ParkingSpotCheckFree(int start, int size, bool[] arr)
        //{
        //    for (int i = start; i < start + size; i++)
        //    {
        //        if (arr[i])
        //        {
        //            return -1;
        //        }
        //    }
        //    return start;
        //}

        //public int GetParkingSpot(int size, bool[] arr)
        //{
        //    int freespot = -1;
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        freespot = ParkingSpotCheckFree(i, size, arr);
        //        if (freespot != -1)
        //        {
        //            for (int j = 0; i < size; i++)
        //            {
        //                arr[i + j] = true;
        //            }
        //            return freespot;
        //        }
        //        else
        //        {
        //            i += size - 1;
        //        }
        //    }
        //    return -1;
        //}

        // 2d, with database
        //public int GetParkingSpotDB(int size)
        //{
        //    var tmp = context.ParkingSpots.LastOrDefault();
        //    if (tmp == null)
        //    {
        //        return -1;
        //    }
        //    int xLength = tmp.X, yLength = tmp.Y;
        //    bool found = false;
        //    for (int i = 0; i < xLength; i++)
        //    {
        //        for (int j = 0; j < yLength; j++)
        //        {
        //            found = true;
        //            for (int k = j; k < j + size; k++)
        //            {
        //                tmp = context.ParkingSpots.FirstOrDefault(s => s.X == i && s.Y == k);
        //                if (tmp == null || tmp.Occupied)
        //                {
        //                    j += size - 1;
        //                    found = false;
        //                    break;
        //                }
        //            }

        //            if (found)
        //            {
        //                for (int k = 0; k < size; k++)
        //                {
        //                    tmp = context.ParkingSpots.FirstOrDefault(s => s.X == i && s.Y == j + k);
        //                    if (tmp != null)
        //                    {
        //                        tmp.Occupied = !tmp.Occupied;
        //                        // Update the database
        //                        context.Entry(tmp).State = EntityState.Modified;
        //                        context.SaveChanges();
        //                    }
        //                }
        //                tmp = context.ParkingSpots.FirstOrDefault(s => s.X == i && s.Y == j);
        //                return tmp.ID;
        //            }
        //        }
        //    }
        //    return -1;
        //}

    }
}