using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        public bool IsPendingAdmin { get; set; }

        [Display(Name = "Requested Role")]
        public string RequestedRole { get; set; }
    }
}