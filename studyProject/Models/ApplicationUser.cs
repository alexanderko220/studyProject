using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace studyProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [StringLength(15)]
        public string FirstName { get; set; }
        [StringLength(15)]
        public string LastName { get; set; }
        [StringLength(30)]
        public string Address { get; set; }
        [StringLength(20)]
        public string City { get; set;}
        [StringLength(20)]
        public string State { get; set; }
        [StringLength(10)]
        public string ZipCode { get; set; }
        public List<WorkOrder> WorkOrders { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string AddressBlock
        {
            get
            {
                string addressBlock = string.Format("{0}<br/>{1},{2},{3}", Address, City, State, ZipCode).Trim();
                return addressBlock == "<br/>," ? string.Empty : addressBlock;
            }
        }
        [NotMapped]
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }

   
}