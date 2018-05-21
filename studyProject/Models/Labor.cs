using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace studyProject.Models
{
    public class Labor
    {
        public int LaborId { get; set; }

        [Index("AK_Labor", 1, IsUnique = true)]
        public int WorkOrderId { get; set; }

        public WorkOrder WorkOrder { get; set; }

        [StringLength(15)]
        [Required]
        [Index("AK_Labor", 2, IsUnique = true)]
        public string ServiceItemCode { get; set; }

        [StringLength(80)]
        [Required]
        public string ServiceItemName { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal LaborHours { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal Rate { get; set; }

       //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ExtendedPrice { get; set; }

        [StringLength(140)]
        public string Notes { get; set; }
    }
}