using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelManagement.Data;
using HotelManagement.Models;
using System.IO;

namespace HotelManagement.Controllers
{
    public class RoomsController : Controller
    {
        private RoomDAL roomDAL = new RoomDAL();

        public ActionResult Index()
        {
            // Call the DAL to get all room records.
            List<Room> rooms = roomDAL.GetAllRooms();
            // Pass the list of rooms to the 'Index' view to be displayed.
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Room room = roomDAL.GetRoomById(id.Value);
            if (room == null)
            {
                // If not found, return an HTTP 404 Not Found error.
                return HttpNotFound();
            }
            // Pass the found room object to the 'Details' view.
            return View(room);
        }


        // GET: Rooms/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            // Simply returns the 'Create' view.
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomNumber,RoomType,Price,IsAvailable,MainImageFile,GalleryImageFiles")] Room room)
        {
            if (ModelState.IsValid)
            {
                // Logic to save the main image file
                if (room.MainImageFile != null && room.MainImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(room.MainImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    room.MainImageFile.SaveAs(path);
                    room.MainImageName = fileName;
                }

                // Logic to save the gallery image files
                if (room.GalleryImageFiles != null && room.GalleryImageFiles.Length > 0)
                {
                    var fileNames = new List<string>();
                    foreach (var file in room.GalleryImageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                            file.SaveAs(path);
                            fileNames.Add(fileName);
                        }
                    }
                    room.GalleryImageNames = string.Join(",", fileNames);
                }

                roomDAL.InsertRoom(room);
                return RedirectToAction("Index");
            }
            return View(room);
        }


        // GET: Rooms/Edit/5
        [Authorize(Roles = "Admin")]
         public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Room room = roomDAL.GetRoomById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
         [HttpPost]
         [ValidateAntiForgeryToken]
         // Include RoomID in Bind for update, as it's needed to identify the record.
         public ActionResult Edit([Bind(Include = "RoomID,RoomNumber,RoomType,Price,IsAvailable,MainImageFile,GalleryImageFiles")] Room room)
         {
            if (ModelState.IsValid)
            {
                // Retain the old image names if no new files are uploaded
                var existingRoom = roomDAL.GetRoomById(room.RoomID);

                if (room.MainImageFile != null && room.MainImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(room.MainImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    room.MainImageFile.SaveAs(path);
                    room.MainImageName = fileName;
                }
                else
                {
                    room.MainImageName = existingRoom.MainImageName;
                }

                if (room.GalleryImageFiles != null && room.GalleryImageFiles.Any(f => f != null && f.ContentLength > 0))
                {
                    var fileNames = new List<string>();
                    foreach (var file in room.GalleryImageFiles)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                            file.SaveAs(path);
                            fileNames.Add(fileName);
                        }
                    }
                    room.GalleryImageNames = string.Join(",", fileNames);
                }
                else
                {
                    room.GalleryImageNames = existingRoom.GalleryImageNames;
                }

                roomDAL.UpdateRoom(room);
                return RedirectToAction("Index");
            }
            return View(room);
         }


        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
         {
            if(id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Room room = roomDAL.GetRoomById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
         }


         // POST: Rooms/Delete/5
        // This action method handles the actual deletion after confirmation.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                roomDAL.DeleteRoom(id);
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting room: " + ex.Message);
                Room room = roomDAL.GetRoomById(id);
                return View(room);
            }
        }

    }
}