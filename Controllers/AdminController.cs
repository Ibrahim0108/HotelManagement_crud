using HotelManagement.Data;
using HotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagement.Controllers
{
     [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        
         private readonly UserDAL userDAL = new UserDAL();
         public ActionResult Dashboard()
         {
             var pendingAdmins = userDAL.GetPendingAdminRequests();
             var allUsers = userDAL.GetAllUsers();

             var viewModel = new AdminDashboardViewModel
             {
                 PendingAdminRequests = pendingAdmins,
                 AllUsers = allUsers
             };

             return View(viewModel);
         }

         [HttpPost]
         public ActionResult ApproveAdmin(int userId)
         {
            userDAL.ApproveAdminRequest(userId);
     
             return RedirectToAction("Dashboard");
         }

         [HttpPost]
         public ActionResult RejectAdmin(int userId)
         {
         userDAL.RejectAdminRequest(userId);

             return RedirectToAction("Dashboard");
         }


    }
}