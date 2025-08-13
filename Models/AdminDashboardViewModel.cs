using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class AdminDashboardViewModel
    {
        public List<User> PendingAdminRequests { get; set; }
        public List<User> AllUsers { get; set; }
    }
}