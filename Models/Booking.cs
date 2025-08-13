using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int GuestId { get; set; }
        public int RoomID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookingDate { get; set; }

        // Properties from the Rooms table, for display
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }

        [Display(Name = "Room Type")]
        public string RoomType { get; set; }

        [Display(Name = "Price Per Night")]
        public decimal PricePerNight { get; set; }

        // Properties from the Guests table, for display
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }

    }
}