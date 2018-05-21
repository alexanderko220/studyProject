using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace studyProject.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }

        [StringLength(15, ErrorMessage = "The Item Code must be 15 characters or shorter.")]
        [Required(ErrorMessage = "You must enter an Item Code")]
        [Index("AK_InventoryItem_InventoryItemCode", IsUnique = true)]
        [Display(Name = "Item Code")]
        public string InventoryItemCode { get; set; }

        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a Name")]
        [Index("AK_InventoryItem_InventoryItemName", IsUnique = true)]
        [Display(Name = "Name")]
        public string InventoryItemName { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        public virtual  Category Category { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}