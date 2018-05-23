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
        [StringLength(8, ErrorMessage = "The account number must be 8 characters or shorter.")]
        [Index("AK_Customer_AccountNumber", IsUnique = true)]
        [Display(Name = "Account #")]
        [Required(ErrorMessage = "You must enter an account number.")]
        public string AccountNumber { get; set; }

        [StringLength(30, ErrorMessage = "The account number must be 30 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a company name.")]
        [Index("AK_Customer_CompanyName", IsUnique = true)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(30, ErrorMessage = "The address must be 30 characters or shorter.")]
        [Required(ErrorMessage = "You must enter an adress.")]
        public string Address { get; set; }

        [StringLength(15, ErrorMessage = "The city must be 15 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a city.")]
        public string City { get; set; }

        [StringLength(15, ErrorMessage = "The state must be 15 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a state.")]
        public string State { get; set; }

        [StringLength(10, ErrorMessage = "The zip code must be 10 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a zip code.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [StringLength(15, ErrorMessage = "The phone must be 15 characters or shorter.")]
        [Required(ErrorMessage = "You must enter a phone.")]
        [Phone]
        public string Phone { get; set; }

        public List<WorkOrder> WorkOrders { get; set; }
    }
}