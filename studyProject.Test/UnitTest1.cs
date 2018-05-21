using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using studyProject.Models;

namespace studyProject.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Create_WorkOrder()
        {
            string description = "test";
            int customerId = 1;
            WorkOrder workOrder = new WorkOrder { Description = description, CustomerId = customerId, WorkOrderStatus = WorkOrderStatus.Created };

            Assert.IsNotNull(workOrder);
            
        }

        
    }
}
