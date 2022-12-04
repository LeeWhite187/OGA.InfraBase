using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OGA.InfraBase.Services
{
    public class UriService :  OGA.SharedKernel.Services.IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(OGA.SharedKernel.QueryHelpers.PaginationFilter filter, string route)
        {
            return Compose_Url_to_Route(route, new List<KeyValuePair<string, string>>()
                                                { new KeyValuePair<string, string> ("pageNumber", filter.pageNumber.ToString()),
                                                new KeyValuePair<string, string> ("pageSize", filter.pageSize.ToString()) });

            //var _enpointUri = new Uri(string.Concat(_baseUri, route));
            //var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.pageNumber.ToString());
            //modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.pageSize.ToString());
            //return new Uri(modifiedUri);
        }


        /// <summary>
        /// Will create a complete URL for the given route.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public Uri Compose_Url_to_Route(string route)
        {
            return Compose_Url_to_Route(route, null);
        }
        /// <summary>
        /// Will create a complete URL for the given route and list of query parameters.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public Uri Compose_Url_to_Route(string route, List<KeyValuePair<string, string>> queryparms)
        {
            string strurl = "";

            // Compose the base url: host and route...
            var _enpointUri = new Uri(new Uri(_baseUri), route);

            // Check that no query parameters were given...
            if(queryparms == null)
                // No query paramters were given.
                return _enpointUri;
            if(queryparms.Count == 0)
                // No query paramters were given.
                return _enpointUri;

            // If here, at least one query parameter was defined.

            // Get the existing query string from the route...
            string existingqs = _enpointUri.Query;

            // See if the given route included a query string...
            if(string.IsNullOrEmpty(existingqs))
            {
                // No existing query string to work with.

                // Add each query parameter to the url...
                strurl = _enpointUri.ToString();
                foreach (var qp in queryparms)
                {
                    strurl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(strurl, qp.Key, qp.Value);
                }
            }
            else
            {
                // An existing query string was given as part of the route.
                // We will make sure to not add any duplicate query parameters.

                // Get a name value dictionary of the query parameters...
                var qd = HttpUtility.ParseQueryString(existingqs);

                // Copy over any passed in query parameters...
                if(qd != null)
                {
                    foreach (var qp in queryparms)
                        qd.Set(qp.Key, qp.Value);
                }

                // Now, we need to recompose the url with the updated query string...
                var ub = new UriBuilder(_enpointUri);
                // Update the query string...
                ub.Query = qd.ToString();

                // And, compose the final url...
                strurl = ub.Uri.ToString();
            }
            // At this point, all parameters are pushed in, and the URL is a string.

            // Convert it back to a URL and return it...
            return new Uri(strurl);
            //// Compose the base url: host and route...
            //var _enpointUri = new Uri(string.Concat(_baseUri, route));

            //// Check that no query parameters were given...
            //if(queryparms == null)
            //    // No query paramters were given.
            //    return _enpointUri;
            //if(queryparms.Count == 0)
            //    // No query paramters were given.
            //    return _enpointUri;

            //// If here, at least one query parameter was defined.

            //// Iterate each query parameter, and add each one to the url...
            //string strurl = _enpointUri.ToString();
            //foreach (var qp in queryparms)
            //{
            //    strurl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(strurl, qp.Key, qp.Value);
            //}
            //// At this point, all parameters are pushed in, and the URL is a string.

            //// Convert it back to a URL and return it...
            //return new Uri(strurl);
        }
    }
}
