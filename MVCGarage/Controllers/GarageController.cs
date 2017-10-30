using MVCGarage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCGarage.Controllers
{
    public class GarageController : Controller
    {
        // GET: Garage
        // Här Ska Listan av alla parkerade bilar + nr av öppna platser.
        public ActionResult Index()
        {
            return View();
        }

        // GET: add
        public ActionResult Add()
        {
            return View();
        }
        // POST: add
        // Skapa en Form som sparar en parkering.
        public ActionResult Add(Vehicle vehicle)
        {
            return View(vehicle);
        }

        // GET: remove
        public ActionResult Remove()
        {
            return View();
        }
        // POST: remove
        // Skapa en Form som sparar en parkering.
        public ActionResult Remove(Vehicle vehicle)
        {
            return View(vehicle);
        }


    }
}