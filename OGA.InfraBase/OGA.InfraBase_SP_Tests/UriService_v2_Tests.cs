using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OGA.InfraBase.Services;
using System.Collections.Generic;
using System.Web;

namespace OGA.InfraBase_Tests
{
    [TestClass]
    public class UriService_Tests
    {
        /* Tests for the Uri Service v2 class.

        // Test  1 - Construct an instance.
         
         
         
         
         */

        // Test  1 - Construct an instance.
        [TestMethod]
        public void Test1()
        {
            var u = new UriService("");
        }

        // Test  2 - Create a simple route.
        [TestMethod]
        public void Test2()
        {
            string baseurl = "http://192.168.1.203:5000";
            string testroute = "api/test/test";

            string expectedurl = baseurl + "/" + testroute;

            var u = new UriService(baseurl);

            var sr = u.Compose_Url_to_Route(testroute);

            if (sr.ToString() != expectedurl)
                Assert.Fail("Wrong url.");
        }

        // Test  3 - Create a route with one query parameter.
        [TestMethod]
        public void Test3()
        {
            string baseurl = "http://192.168.1.203:5000";
            string testroute = "api/test/test";

            string queryparmname = "ffff";
            string queryparmvalue = "gggg";

            string expectedurl = baseurl + "/" + testroute + "?" + queryparmname + "=" + queryparmvalue;

            var u = new UriService(baseurl);

            // Create the query parameter list...
            var qpl = new List<KeyValuePair<string, string>>();

            qpl.Add(new KeyValuePair<string, string>(queryparmname, queryparmvalue));

            var sr = u.Compose_Url_to_Route(testroute, qpl);

            if (sr.ToString() != expectedurl)
                Assert.Fail("Wrong url.");
        }

        // Test  4 - Create a route with two query parameters.
        [TestMethod]
        public void Test4()
        {
            string baseurl = "http://192.168.1.203:5000";
            string testroute = "api/test/test";

            string queryparmname = "ffff";
            string queryparmvalue = "gggg";
            string queryparmname2 = "hhhh";
            string queryparmvalue2 = "iiii";

            string expectedurl = baseurl + "/" + testroute + "?" + queryparmname + "=" + queryparmvalue + "&" + queryparmname2 + "=" + queryparmvalue2;

            var u = new UriService(baseurl);

            // Create the query parameter list...
            var qpl = new List<KeyValuePair<string, string>>();

            qpl.Add(new KeyValuePair<string, string>(queryparmname, queryparmvalue));
            qpl.Add(new KeyValuePair<string, string>(queryparmname2, queryparmvalue2));

            var sr = u.Compose_Url_to_Route(testroute, qpl);

            if (sr.ToString() != expectedurl)
                Assert.Fail("Wrong url.");
        }

        // Test  5 - Create a paginated url with a pre-existing query parameter and a pagefilter.
        [TestMethod]
        public void Test5()
        {
            string originalUrl = "http://192.168.1.203:5000/api/test/test?hhhh=iiii";

            // Normally done in startup.cs....
            string baseurl = "http://192.168.1.203:5000";
            var u = new UriService(baseurl);

            // Get the route from the request...
            string route = "api/test/test";

            // Compose the query string...
            string queryparmname = "ffff";
            string queryparmvalue = "gggg";
            string querystring_fromrequest = "?" + queryparmname + "=" + queryparmvalue;

            // Compose the route and query string...
            string route_withquery = route + querystring_fromrequest;

            // Done inside the Pagination class...
            var sr = u.GetPageUri(new SharedKernel.QueryHelpers.PaginationFilter(1, 10), route_withquery);


            // Now, we need to confirm the query string has all the required values...

            // Recover the final query string as a dictionary...
            var qs = sr.Query;
            var qd = HttpUtility.ParseQueryString(qs);

            if (qd[queryparmname] != queryparmvalue)
                Assert.Fail("Wrong query parameter.");

            if (qd["pageNumber"] != "1")
                Assert.Fail("Wrong query parameter.");

            if (qd["pageSize"] != "10")
                Assert.Fail("Wrong query parameter.");
        }

        // Test  6 - Create a paginated url with a pre-existing query parameters and page numbering.
        [TestMethod]
        public void Test6()
        {
            string originalUrl = "http://192.168.1.203:5000/api/test/test?hhhh=iiii";

            // Normally done in startup.cs....
            string baseurl = "http://192.168.1.203:5000";
            var u = new UriService(baseurl);

            // Get the route from the request...
            string route = "api/test/test";

            // Compose the query string...
            string queryparmname = "ffff";
            string queryparmvalue = "gggg";
            string querystring_fromrequest = "?" + queryparmname + "=" + queryparmvalue;
            // Add in some pre-existing page numbering...
            querystring_fromrequest = querystring_fromrequest + "&" + "pageNumber" + "=" + "3";
            querystring_fromrequest = querystring_fromrequest + "&" + "pageSize" + "=" + "2";

            // Compose the route and query string...
            string route_withquery = route + querystring_fromrequest;

            // Done inside the Pagination class...
            var sr = u.GetPageUri(new SharedKernel.QueryHelpers.PaginationFilter(1, 10), route_withquery);


            // Now, we need to confirm the query string has all the required values...

            // Recover the final query string as a dictionary...
            var qs = sr.Query;
            var qd = HttpUtility.ParseQueryString(qs);

            if(qd.Keys.Count != 3)
                Assert.Fail("Wrong query parameter.");

            if (qd[queryparmname] != queryparmvalue)
                Assert.Fail("Wrong query parameter.");

            if (qd["pageNumber"] != "1")
                Assert.Fail("Wrong query parameter.");

            if (qd["pageSize"] != "10")
                Assert.Fail("Wrong query parameter.");
        }

        // Test  7 - Create a paginated url with a pre-existing query parameters and page numbering with different casing.
        [TestMethod]
        public void Test7()
        {
            string originalUrl = "http://192.168.1.203:5000/api/test/test?hhhh=iiii";

            // Normally done in startup.cs....
            string baseurl = "http://192.168.1.203:5000";
            var u = new UriService(baseurl);

            // Get the route from the request...
            string route = "api/test/test";

            // Compose the query string...
            string queryparmname = "ffff";
            string queryparmvalue = "gggg";
            string querystring_fromrequest = "?" + queryparmname + "=" + queryparmvalue;
            // Add in some pre-existing page numbering...
            querystring_fromrequest = querystring_fromrequest + "&" + "pagenumber" + "=" + "3";
            querystring_fromrequest = querystring_fromrequest + "&" + "pagesize" + "=" + "2";

            // Compose the route and query string...
            string route_withquery = route + querystring_fromrequest;

            // Done inside the Pagination class...
            var sr = u.GetPageUri(new SharedKernel.QueryHelpers.PaginationFilter(1, 10), route_withquery);


            // Now, we need to confirm the query string has all the required values...

            // Recover the final query string as a dictionary...
            var qs = sr.Query;
            var qd = HttpUtility.ParseQueryString(qs);

            if(qd.Keys.Count != 3)
                Assert.Fail("Wrong query parameter.");

            if (qd[queryparmname] != queryparmvalue)
                Assert.Fail("Wrong query parameter.");

            if (qd["pageNumber"] != "1")
                Assert.Fail("Wrong query parameter.");

            if (qd["pageSize"] != "10")
                Assert.Fail("Wrong query parameter.");
        }
    }
}
