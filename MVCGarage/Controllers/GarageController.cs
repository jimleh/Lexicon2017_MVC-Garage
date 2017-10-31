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
        GarageRepository repo = new GarageRepository();

        // GET: Garage
        // Här Ska Listan av alla parkerade bilar + nr av öppna platser.
        public ActionResult Index()
        {
            return View(repo.getAllVehicles());
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
        public ActionResult Add(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                repo.CheckInVehicle(vehicle);
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: remove
        public ActionResult Remove()
        {
            return View();
        }

        // POST: remove
        // Tar bort en vehicle från parkeringen 
        public ActionResult CheckOut(Vehicle vehicle)
        {
            return View(vehicle);
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