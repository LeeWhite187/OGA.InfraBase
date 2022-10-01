using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OGA.SharedKernel.QueryHelpers;
using OGA.SharedKernel.Services;

namespace OGA.DomainBase.QueryHelpers
{
    /// <summary>
    /// Used to create a paginated list of a particular DTO type.
    /// Has a static method as its primary usage interface, that returns the pagination instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get
            {
                if (RowCount > 0)
                    return (CurrentPage - 1) * PageSize + 1;
                else
                    return 0;
            }
        }
         public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < PageCount);
            }
        }
        
        public PaginatedList(List<T> data, int currentPage, int pageSize)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;

            this.AddRange(data);
        }

        /// <summary>
        /// Call this static method to create a paginated list from an IQueryable instance.
        /// It will do the skip/take for the appropriate page of data to return.
        /// And, will populate the page count and other metrics.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        static public async Task<IPaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            // Check that the page index is valid...
            if (pageSize < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("pageSize invalid. Must be positive.");

            // Check that the page index is valid...
            if (pageIndex < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Must be positive.");

            // Count how many result entries in the query...
            var queryresult_count = await source.CountAsync();

            // Determine how many pages required to display the query results...
            var pagecount = (int) Math.Ceiling((double) queryresult_count / pageSize);

            //// See if the page index is out of range, high...
            //if(pageIndex > pagecount)
            //    throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Out of range, high.");

            // Get the desired page of items from the query results...
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            // Return the paginated list to the caller...
            return new PaginatedList<T>(items, pageIndex, pageSize) { RowCount = queryresult_count, PageCount = pagecount };
        }
    }

    /// <summary>
    /// Extends the PaginatedList type with URL links that can be used as button links.
    /// Creates the same paginated list of a particular DTO type, as the PaginatedList.
    /// Has a static method as its primary usage interface, that returns the pagination instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList_withURL<T> : PaginatedList<T>, IPaginatedList_withURL<T>
    {
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PaginatedList_withURL(List<T> data, int currentPage, int pageSize): base(data, currentPage, pageSize)
        {
            // These two properties are already set in the base.
            // No need to set them here.
            //this.CurrentPage = currentPage;
            //this.PageSize = pageSize;

            // The range was already set in the base class.
            // Doing it again, here, only creates duplicate entries.
            //this.AddRange(data);
        }

        public void Setup_URLs(IUriService urisvc, string route)
        {
            // Calculate urls for the page...
            Uri firstpage = urisvc.GetPageUri(new PaginationFilter(1, this.PageSize), route);
            Uri lastpage = urisvc.GetPageUri(new PaginationFilter(this.PageCount, this.PageSize), route);
            Uri prevpage = this.CurrentPage - 1 >= 1 && this.CurrentPage <= this.PageCount ? urisvc.GetPageUri(new PaginationFilter(this.CurrentPage - 1, this.PageSize), route) : null;
            Uri nextpage = this.CurrentPage >= 1 && this.CurrentPage < this.PageCount ? urisvc.GetPageUri(new PaginationFilter(this.CurrentPage + 1, this.PageSize), route) : null;
        }

        /// <summary>
        /// Call this static method to create a paginated list from an IQueryable instance.
        /// It will do the skip/take for the appropriate page of data to return.
        /// And, will populate the page count and other metrics.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        static public new async Task<IPaginatedList_withURL<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, IUriService urisvc, string route)
        {
            // Check that the page index is valid...
            if (pageSize < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("pageSize invalid. Must be positive.");

            // Check that the page index is valid...
            if (pageIndex < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Must be positive.");

            // Count how many result entries in the query...
            var queryresult_count = await source.CountAsync();

            // Determine how many pages required to display the query results...
            var pagecount = (int) Math.Ceiling((double) queryresult_count / pageSize);

            //// See if the page index is out of range, high...
            //if(pageIndex > pagecount)
            //    throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Out of range, high.");

            // Calculate urls for the page...
            Uri firstpage = urisvc.GetPageUri(new PaginationFilter(1, pageSize), route);
            Uri lastpage = urisvc.GetPageUri(new PaginationFilter(pagecount, pageSize), route);
            Uri prevpage = pageIndex - 1 >= 1 && pageIndex <= pagecount ? urisvc.GetPageUri(new PaginationFilter(pageIndex - 1, pageSize), route) : null;
            Uri nextpage = pageIndex >= 1 && pageIndex < pagecount ? urisvc.GetPageUri(new PaginationFilter(pageIndex + 1, pageSize), route) : null;

            // Get the desired page of items from the query results...
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            // Return the paginated list to the caller...
            return new PaginatedList_withURL<T>(items, pageIndex, pageSize) {
                RowCount = queryresult_count,
                PageCount = pagecount,
                FirstPage = firstpage,
                LastPage = lastpage,
                PreviousPage = prevpage,
                NextPage = nextpage
            };
        }
    }
}
