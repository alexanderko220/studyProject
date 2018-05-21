using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace studyProject.Models
{
    public class Part
    {
        public int PartId { get; set; }

        [Index("AK_Part", 1, IsUnique = true)]
        public int WorkOrderId { get; set; }

        public WorkOrder WorkOrder { get; set; }

        [StringLength(15)]
        [Required]
        [Index("AK_Part", 2 ,IsUnique = true)]
        public string InventoryItemCode { get; set; }

        [StringLength(80)]
        [Required]
        public string InventoryItemName { get; set; }
        public int Quantity { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal UnitPrice { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ExtendedPrice { get; set; }

        [StringLength(140)]
        public string Notes { get; set; }

        public bool IsInstalled { get; set; }
    }
}