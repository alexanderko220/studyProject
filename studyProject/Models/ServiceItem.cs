using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace studyProject.Models
{
    public class ServiceItem
    {
        
        public int ServiceItemId { get; set; }

        [StringLength(15, ErrorMessage = "The Item Code must be 15 characters or shorter.")]
        [Required(ErrorMessage = "You must enter an Item Code")]
        [Index("AK_ServiceItem_ServiceItemCode", IsUnique = true)]
        [Display(Name = "Item Code")]
        public string ServiceItemCode { get; set; }

        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a Name")]
        [Index("AK_ServiceItem_ServiceItemName", IsUnique = true)]
        [Display(Name = "Name")]
        public string ServiceItemName { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Rate { get; set; }
    }
}