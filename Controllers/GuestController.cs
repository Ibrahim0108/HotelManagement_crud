using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GuestController : Controller
    {
        private GuestDAL guestDAL = new GuestDAL();
        // GET: Guest
        public ActionResult Index()
        {
            List<Guest> guests = guestDAL.GetAllGuests();
            return View(guests);  
        }

        public ActionResult Details(int id)
        {
            Guest guest = guestDAL.GetGuestById(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Guest guest)
        {
            if (ModelState.IsValid)
            {
                guestDAL.CreateGuest(guest);
                return RedirectToAction("Index");
            }
            return View(guest);
        }


        public ActionResult Edit(int id)
        {
            Guest guest = guestDAL.GetGuestById(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guest guest)
        {
            if (ModelState.IsValid)
            {
                guestDAL.UpdateGuest(guest);
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        // GET: Guest/Delete/5
        public ActionResult Delete(int id)
        {
            Guest guest = guestDAL.GetGuestById(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            guestDAL.DeleteGuest(id);
            return RedirectToAction("Index");
        }

    }
}