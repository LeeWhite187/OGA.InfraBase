using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OGA.DomainBase_Tests
{
    /* Paginated List Tests:
        //  1.  Paginated Query for  1 record  with page size of 10.
        //  2.  Paginated Query for  9 records with page size of 10.
        //  3.  Paginated Query for 10 records with page size of 10.
        //  4.  Paginated Query for 11 records with page size of 10.
        //  5.  Paginated Query for 19 records with page size of 10.
        //  6.  Paginated Query for 20 records with page size of 10.
        //  7.  Paginated Query for 21 records with page size of 10.
        //  8.  Paginated Query for 21 records with page size of 10 and page index of 2.
        //  9.  Paginated Query for 21 records with page size of 10 and page index of 3.
        // 10.  Paginated Query pageindex = 0.
        // 11.  Paginated Query pageSize = 0.
        // 12.  Paginated Query zero records.
     */
        [TestClass]
    public class PaginatedList_Int32_Tests : OGA.Testing.Lib.Test_Base
    {
        static private string _dbname = System.Guid.NewGuid().ToString();


        #region Setup

        [TestInitialize]
        public override void Setup()
        {
            // Call this first...
            base.Setup();

            Enable_AllLoggingLevels();

            Print_Enabled_LogLevels();

            Load_AssemblyHelperInstance();

            //// Runs before each test. (Optional)
        }

        [TestCleanup]
        public override void TearDown()
        {
            // Runs after each test. (Optional)


            // Call this last...
            base.TearDown();
        }

        #endregion


        //  1.  Paginated Query for  1 record  with page size of 10.
        [TestMethod]
        public void Test1()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 1; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 1)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 1)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != false)
                    Assert.Fail("Value not correct.");
            }
        }

        //  2.  Paginated Query for  9 records with page size of 10.
        [TestMethod]
        public void Test2()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 9; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 1)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 9)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 9)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != false)
                    Assert.Fail("Value not correct.");
            }
        }

        //  3.  Paginated Query for 10 records with page size of 10.
        [TestMethod]
        public void Test3()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 10; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 1)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 10)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 10)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != false)
                    Assert.Fail("Value not correct.");
            }
        }

        //  4.  Paginated Query for 11 records with page size of 10.
        [TestMethod]
        public void Test4()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 11; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 2)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 11)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 10)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != true)
                    Assert.Fail("Value not correct.");
            }
        }

        //  5.  Paginated Query for 19 records with page size of 10.
        [TestMethod]
        public void Test5()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 19; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 2)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 19)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 10)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != true)
                    Assert.Fail("Value not correct.");
            }
        }

        //  6.  Paginated Query for 20 records with page size of 10.
        [TestMethod]
        public void Test6()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 20; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 2)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 20)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 10)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != true)
                    Assert.Fail("Value not correct.");
            }
        }

        //  7.  Paginated Query for 21 records with page size of 10.
        [TestMethod]
        public void Test7()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 21; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 3)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 21)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 10)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != true)
                    Assert.Fail("Value not correct.");
            }
        }

        //  8.  Paginated Query for 21 records with page size of 10 and page index of 2.
        [TestMethod]
        public void Test8()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 21; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 2, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 2)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 3)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 21)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 11)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 20)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != true)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != true)
                    Assert.Fail("Value not correct.");
            }
        }

        //  9.  Paginated Query for 21 records with page size of 10 and page index of 3.
        [TestMethod]
        public void Test9()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 21; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 3, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 3)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 3)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 21)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 21)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 21)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != true)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != false)
                    Assert.Fail("Value not correct.");
            }
        }

        // 10.  Paginated Query pageindex = 0.
        [TestMethod]
        public void Test10()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 21; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test..
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                try
                {
                    var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 0, 10).Result;

                    Assert.Fail("An exception should have been thrown.");
                }
                catch (Exception e)
                {
                    // Since we are running inside unit testing, our exception type will be an inner exception of the exception we receive.

                    // Get the inner exception...
                    var ee = e.InnerException;

                    // Check its type...
                    var etn = ee.GetType().Name;
                    if (etn != "BusinessRuleBrokenException")
                        Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                                    e.GetType(), e.Message));

                    // Check the exception message...
                    if(ee.Message != "PageIndex invalid. Must be positive.")
                        Assert.Fail("Incorrect exception messag.");
                }
            }
        }

        // 11.  Paginated Query pageSize = 0.
        [TestMethod]
        public void Test11()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 10; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test..
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            //where b.Val2 > 1000
                            select b;

                try
                {
                    var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 0).Result;

                    Assert.Fail("An exception should have been thrown.");
                }
                catch (Exception e)
                {
                    // Since we are running inside unit testing, our exception type will be an inner exception of the exception we receive.

                    // Get the inner exception...
                    var ee = e.InnerException;

                    // Check its type...
                    var etn = ee.GetType().Name;
                    if (etn != "BusinessRuleBrokenException")
                        Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}",
                                    e.GetType(), e.Message));

                    // Check the exception message...
                    if(ee.Message != "pageSize invalid. Must be positive.")
                        Assert.Fail("Incorrect exception messag.");
                }
            }
        }

        // 12.  Paginated Query zero records.
        [TestMethod]
        public void Test12()
        {
            string dbname = System.Guid.NewGuid().ToString();

            // Insert seed data into the database using one instance of the context...
            using (var context = Get_TestContext(dbname))
            {
                // Populate the list...
                for(int x = 0; x < 10; x++)
                {
                    OGA.DomainBase_Tests.TestClass_Int32Id tc = new OGA.DomainBase_Tests.TestClass_Int32Id();
                    tc.Val1 = x.ToString();
                    tc.Val2 = x;

                    context.TestData.Add(tc);
                }

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test..
            using (var context = Get_TestContext(dbname))
            {
                // Now, do a simple query for some portion of the list...
                var query = from b in context.TestData
                            where b.Val2 > 1000
                            select b;

                // Declare an instance of our test class...
                var pl = OGA.InfraBase.QueryHelpers.PaginatedList<OGA.DomainBase_Tests.TestClass_Int32Id>.CreateAsync(query, 1, 10).Result;

                // See how many elements we have...
                if (pl.PageSize != 10)
                    Assert.Fail("Value not correct.");
                if (pl.CurrentPage != 1)
                    Assert.Fail("Value not correct.");
                if (pl.PageCount != 0)
                    Assert.Fail("Value not correct.");
                if (pl.RowCount != 0)
                    Assert.Fail("Value not correct.");
                if (pl.FirstRowOnPage != 0)
                    Assert.Fail("Value not correct.");
                if (pl.LastRowOnPage != 0)
                    Assert.Fail("Value not correct.");
                if (pl.HasPreviousPage != false)
                    Assert.Fail("Value not correct.");
                if (pl.HasNextPage != false)
                    Assert.Fail("Value not correct.");
            }
        }

        static private OGA.DomainBase_Tests.TestDbContext_Int32Id Get_TestContext()
        {
            return Get_TestContext(_dbname);
        }
        static private OGA.DomainBase_Tests.TestDbContext_Int32Id Get_TestContext(string dbname)
        {
            var options = new DbContextOptionsBuilder<OGA.DomainBase_Tests.TestDbContext_Int32Id>()
                .UseInMemoryDatabase(databaseName: dbname)
                .Options;

            var context = new OGA.DomainBase_Tests.TestDbContext_Int32Id(options);

            return context;
        }
    }
}
