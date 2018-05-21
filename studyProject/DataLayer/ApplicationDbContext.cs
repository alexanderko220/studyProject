using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using studyProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace studyProject.DataLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) { }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Labor> Labors { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }

        

        // for testing 
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<WorkOrder>()
                .HasRequired(wo => wo.CurrentWorker).WithMany(au => au.WorkOrders).WillCascadeOnDelete(false);
            modelBuilder.Entity<WorkOrder>()
                .HasRequired(wo => wo.Customer).WithMany(c => c.WorkOrders).WillCascadeOnDelete(false);
            modelBuilder.Entity<InventoryItem>()
               .HasRequired(ii => ii.Category).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Category>()
                .HasOptional(c => c.Parent).WithMany(c => c.Children).HasForeignKey(c => c.ParentCategoryId).WillCascadeOnDelete(false);




        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();

        }

        
    }


}