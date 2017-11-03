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
        private int HourlyFee;


        //private bool[] parkingspots = new bool[100];
        private bool[,,] parkingSpots = new bool[2, 10, 25];

        public GarageRepository()
        {
            context = new GarageContext();
            currentSort = SearchOption.RefId;
            sortAscending = false;
            InitParkingSpots();
            HourlyFee = 50;
            //for (int i = 0; i < parkingspots.Length; i++)
            //{
            //    parkingspots[i] = false;
            //}
        }

        // Find all the occupied parking slots
        protected void InitParkingSpots()
        {
            int index = 0;
            for (int i = 0; i < parkingSpots.GetLength(0); i++)
            {
                for (int j = 0; j < parkingSpots.GetLength(1); j++)
                {
                    for (int k = 0; k < parkingSpots.GetLength(2); k++)
                    {
                        index++;
                        var tmp = context.Vehicles.FirstOrDefault(v => v.ParkingSpot == index);
                        if (tmp != null)
                        {
                            for (int l = 0; l < tmp.Size; l++)
                            {
                                parkingSpots[i, j, k + l] = true;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<Vehicle> getAllVehicles()
        {
            List<Vehicle> vlist = context.Vehicles.ToList();
            foreach (Vehicle v in vlist) {
                v.updateFee(HourlyFee);
            }
            return vlist;
        }

        public Vehicle getSpecificVehicle(int? id)
        {
            Vehicle v = context.Vehicles.FirstOrDefault(a => a.ParkingID == id);
            v.updateFee(HourlyFee);
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
            else
            {
                query = query.Where(a => a.DateCheckout != null);
            }

            if (search == null)
            {
                result = query;
            }
            else { 
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
            }

            foreach (Vehicle v in result)
            {
                v.updateFee(HourlyFee);
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


            foreach (Vehicle v in query)
            {
                v.updateFee(HourlyFee);
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
            ClearParkingSpotsForVehicle(vehicle.ParkingID, vehicle.Size);
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
                    for (int k = 0; k < parkingSpots.GetLength(2); k++)
                    {
                        index++;
                        if (index == id)
                        {
                            for (int l = 0; l < size; l++)
                            {
                                parkingSpots[i, j, k + l] = false;
                            }
                        }
                    }
                }
            }
        }

        // Modified version of the previous ParkingSpotCheckFree method
        private bool ParkingSpotCheckFree(int x, int y, int start, int size)
        {
            for (int i = start; i < start + size; i++)
            {
                if (i > parkingSpots.GetLength(2) || parkingSpots[x, y, i])
                {
                    return false;
                }
            }
            return true;
        }

        // 3d, but with an array instead of using the database
        public int GetParkingSpot(int size)
        {
            int index = 1;

            for(int i = 0; i < parkingSpots.GetLength(0); i++)
            {
                for(int j = 0; j < parkingSpots.GetLength(1); j++)
                {
                    for (int k = 0; k < parkingSpots.GetLength(2); k++)
                    {
                        if (ParkingSpotCheckFree(i, j, k, size))
                        {
                            for (int l = 0; l < size; l++)
                            {
                                parkingSpots[i, j, k + l] = true;
                            }
                            return index;
                        }
                        index++;
                    }
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