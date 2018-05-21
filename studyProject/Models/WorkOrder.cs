using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace studyProject.Models
{
    public class WorkOrder
    {
        public int WorkOrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime OrderDateTime { get; set; }

        public DateTime? TargetDateTime { get; set; }
        public DateTime? DropDeadDateTime { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
        public WorkOrderStatus WorkOrderStatus { get; set; }

        //[RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Total { get; set; }

        [StringLength(120)]
        public string CertificationRequirements { get; set; }
     
        public ApplicationUser CurrentWorker { get; set; }

        [Required]
        public string CurrentWorkerId { get; set; }
    }


    public enum WorkOrderStatus
    {
        Created = 0,
        InProcess = 10,
        Rework = 15,
        Submitted = 20,
        Approved = 30,
        Canceled = -10,
        Rejected = -20
    }
}