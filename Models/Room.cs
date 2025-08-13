using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Room
    {
        [Key]
        [Display(Name = "Room ID")] 
        public int RoomID { get; set; }


        [Required(ErrorMessage = "Room Number is required.")]
        [StringLength(10, ErrorMessage = "Room Number cannot exceed 10 characters.")]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }


        [Required(ErrorMessage = "Room Type is required.")]
        [StringLength(50, ErrorMessage = "Room Type cannot exceed 50 characters.")]
        [Display(Name = "Room Type")]
        public string RoomType { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        // [Range]: Ensures the numeric value falls within a specified range.
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00.")]
        [DataType(DataType.Currency)] // [DataType]: Provides a hint for UI rendering (e.g., currency formatting).
        [Display(Name = "Price Per Night")]
        public decimal Price { get; set; }


        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        // NEW PROPERTIES for image handling
        [Display(Name = "Main Room Image")]
        public HttpPostedFileBase MainImageFile { get; set; } // Used for file upload in the form

        public string MainImageName { get; set; } // The name stored in the database

        [Display(Name = "Gallery Images")]
        public HttpPostedFileBase[] GalleryImageFiles { get; set; } // Used for multi-file upload

        public string GalleryImageNames { get; set; } // Comma-separated string in the database


        // Helper properties for the view
        public List<string> GalleryImageUrls
        {
            get
            {
                if (!string.IsNullOrEmpty(GalleryImageNames))
                {
                    // Split the comma-separated string into a list of URLs
                    return GalleryImageNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(name => "/Content/Images/" + name.Trim())
                                            .ToList();
                }
                return new List<string>();
            }
        }

        public string MainImageUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(MainImageName))
                {
                    return "/Content/Images/" + MainImageName;
                }
                // Fallback image if no main image is set
                return "/Content/Images/placeholder.jpg";
            }
        }
    }
}