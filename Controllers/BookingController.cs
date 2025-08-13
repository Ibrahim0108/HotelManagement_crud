using HotelManagement.Data;
using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagement.Controllers
{
    public class BookingController : Controller
    {
        private BookingDAL bookingDAL = new BookingDAL();

        // GET: Booking
        public ActionResult Index()
        {
            List<Booking> bookings = bookingDAL.GetAllBookings();
            return View(bookings);
        }

        public ActionResult Details(int id)
        {
            Booking booking = bookingDAL.GetBookingById(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                bookingDAL.CreateBooking(booking);
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        public ActionResult Edit(int id)
        {
            Booking booking = bookingDAL.GetBookingById(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                bookingDAL.UpdateBooking(booking);
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        public ActionResult Delete(int id)
        {
            Booking booking = bookingDAL.GetBookingById(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bookingDAL.DeleteBooking(id);
            return RedirectToAction("Index");
        }

        // Updated GET: Booking/ConfirmBooking
        // Displays a summary of the booking, calculates the total, and shows a form for customer details.
        public ActionResult ConfirmBooking(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            // First, retrieve the room details to get the price.
            var roomDAL = new RoomDAL(); // Assuming you have a RoomDAL to get room details
            Room room = roomDAL.GetRoomById(roomId);

            if (room == null)
            {
                return HttpNotFound();
            }

            // Calculate the number of nights.
            var numberOfNights = (checkOutDate - checkInDate).TotalDays;

            // Calculate the total amount based on the number of nights and the room's price.
            decimal totalAmount = room.Price * (decimal)numberOfNights;

            var newBooking = new Booking
            {
                RoomID = roomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                TotalAmount = totalAmount,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                PricePerNight = room.Price // Use the correct property
            };

            return View(newBooking);
        }
    }
}