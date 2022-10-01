using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Compose the base url: host and route...
            var _enpointUri = new Uri(string.Concat(_baseUri, route));

            // Check that no query parameters were given...
            if(queryparms == null)
                // No query paramters were given.
                return _enpointUri;
            if(queryparms.Count == 0)
                // No query paramters were given.
                return _enpointUri;

            // If here, at least one query parameter was defined.

            // Iterate each query parameter, and add each one to the url...
            string strurl = _enpointUri.ToString();
            foreach (var qp in queryparms)
            {
                strurl = QueryHelpers.AddQueryString(strurl, qp.Key, qp.Value);
            }
            // At this point, all parameters are pushed in, and the URL is a string.

            // Convert it back to a URL and return it...
            return new Uri(strurl);
        }
    }
}
