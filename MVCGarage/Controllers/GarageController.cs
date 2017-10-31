using MVCGarage.Models;
using MVCGarage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCGarage.Controllers
{


    public class GarageController : Controller
    {
        private bool[] option = {false,false,false,false,false};
        private GarageRepository repo = new GarageRepository();
         
        // GET: Garage
        // Här Ska Listan av alla parkerade bilar + nr av öppna platser.
        public ActionResult Index( string search = null, bool _refid = false, bool _regnr = false, bool _owner = false, bool _date = false, bool _checkout = false)
        {
            IEnumerable<Vehicle> vehicles;
            option[(int)SearchOption.RefId] = false;
            option[(int)SearchOption.RegNr] = false;
            option[(int)SearchOption.Owner] = false;
            option[(int)SearchOption.Date] = false;
            option[(int)SearchOption.Checkout] = false;

            if (search == null)
            {
                vehicles = repo.getFilteredVehicles(null,option);
            }
            else
            {
                option[(int)SearchOption.RefId] = _refid;
                option[(int)SearchOption.RegNr] = _regnr;
                option[(int)SearchOption.Owner] = _owner;
                option[(int)SearchOption.Date] = _date;
                option[(int)SearchOption.Checkout] = _checkout;
                vehicles = repo.getFilteredVehicles(search, option);
            }
            return View(vehicles);
        }

        public ActionResult Archive( string search = null, bool _refid = false, bool _regnr = false, bool _owner = false, bool _date = false)
        {
            IEnumerable<Vehicle> vehicles;
            option[(int)SearchOption.RefId] = false;
            option[(int)SearchOption.RegNr] = false;
            option[(int)SearchOption.Owner] = false;
            option[(int)SearchOption.Date] = false;
            option[(int)SearchOption.Checkout] = true;

            if (search == null)
            {
                vehicles = repo.getFilteredVehicles(null, option);
            }
            else
            {
                option[(int)SearchOption.RefId] = _refid;
                option[(int)SearchOption.RegNr] = _regnr;
                option[(int)SearchOption.Owner] = _owner;
                option[(int)SearchOption.Date] = _date;
                option[(int)SearchOption.Checkout] = true;
                vehicles = repo.getFilteredVehicles(search, option);
            }
            return View(vehicles);
        }



        // GET: add
        public ActionResult CheckIn()
        {
            return View();
        }

        // POST: add
        // Skapa en Form som sparar en parkering.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.ParkingSpot = repo.GetParkingSpot();
                repo.CheckInVehicle(vehicle);
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }


        public ActionResult CheckOut()
        {
            return View();
        }

        // Tar bort en vehicle från parkeringen 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(int? id)
        {
            Vehicle vehicle = repo.getSpecificVehicle(id);
            repo.CheckOutVehicle(vehicle);
            return RedirectToAction("Index");
        }

        public ActionResult Options(Vehicle vehicle)
        {
            return View(vehicle);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = repo.getSpecificVehicle(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                repo.EditVehicle(vehicle);
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = repo.getSpecificVehicle(id);

            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = repo.getSpecificVehicle(id);
            repo.DeleteVehicle(vehicle);
            return RedirectToAction("Index");
        }




    }
}