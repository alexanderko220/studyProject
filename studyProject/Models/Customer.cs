using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace studyProject.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [StringLength(8)]
        [Required]
        [Index("AK_Customer_AccountNumber", IsUnique = true)]
        public string AccountNumber { get; set; }

        [StringLength(30)]
        [Required]
        [Index("AK_Customer_CompanyName", IsUnique = true)]
        public string CompanyName { get; set; }

        [StringLength(30)]
        [Required]
        public string Address { get; set; }

        [StringLength(15)]
        [Required]
        public string City { get; set; }

        [StringLength(15)]
        [Required]
        public string State { get; set; }

        [StringLength(10)]
        [Required]
        public string ZipCode { get; set; }

        [StringLength(15)]
        [Required]
        [Phone]
        public string Phone { get; set; }

        public List<WorkOrder> WorkOrders { get; set; }
    }
}