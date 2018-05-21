namespace studyProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identity : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Customers", "CompanyName", unique: true, name: "AK_Customer_CompanyName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", "AK_Customer_CompanyName");
        }
    }
}
