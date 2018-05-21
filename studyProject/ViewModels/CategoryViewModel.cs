using studyProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace studyProject.ViewModels
{
    public class CategoryViewModel
    {
        
        public int Id { get; set; }
        
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
        

        [Display(Name = "Category")]
        [StringLength(20, ErrorMessage = "Category names must be 20 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a category name.")]
        public string CategoryName { get; set; }


        public virtual List<InventoryItem> InventoryItems { get; set; }

        
    }
}