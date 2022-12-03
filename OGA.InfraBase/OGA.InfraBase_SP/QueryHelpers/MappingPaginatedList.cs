using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OGA.DomainBase.QueryHelpers;
using OGA.SharedKernel.QueryHelpers;
using OGA.SharedKernel.Services;

namespace OGA_WebAPI_Base.QueryHelpers
{
    /// <summary>
    /// Used to create a paginated list of a particular DTO type.
    /// Has a static method as its primary usage interface, that returns the pagination instance.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class MappingPaginatedList<TResult>: OGA.DomainBase.QueryHelpers.IMappingPaginatedList<TResult>
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

        public List<TResult> Data { get; set; }

        public MappingPaginatedList(List<TResult> data, int currentPage, int pageSize)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;

            this.Data = new List<TResult>();
            this.Data.AddRange(data);
        }

        /// <summary>
        /// Call this static method to create a paginated list from an IQueryable instance.
        /// It will do the skip/take for the appropriate page of data to return.
        /// And, will populate the page count and other metrics.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mapper">IMapper instance needed here</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        static public async Task<MappingPaginatedList<TResult>> CreateAsync<TSource>(IQueryable<TSource> source,
                                                                                     IMapper mapper,
                                                                                     int pageNumber,
                                                                                     int pageSize)
        {
            // Check that the page index is valid...
            if (pageSize < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("pageSize invalid. Must be positive.");

            // Check that the page index is valid...
            if (pageNumber < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Must be positive.");

            // Count how many result entries in the query...
            var queryresult_count = await source.CountAsync();

            // Determine how many pages required to display the query results...
            var pagecount = (int) Math.Ceiling((double) queryresult_count / pageSize);

            //// See if the page index is out of range, high...
            //if(pageIndex > pagecount)
            //    throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Out of range, high.");

            // Get the desired page of items from the query results...
            var items = await source.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .Select(ufc => mapper.Map<TResult>(ufc))
                                .ToListAsync();

            // Return the paginated list to the caller...
            return new MappingPaginatedList<TResult>(items, pageNumber, pageSize) { RowCount = queryresult_count, PageCount = pagecount };
        }
    }

    /// <summary>
    /// Extends the PaginatedList type with URL links that can be used as button links.
    /// Creates the same paginated list of a particular DTO type, as the PaginatedList.
    /// Has a static method as its primary usage interface, that returns the pagination instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappingPaginatedList_withURL<TResult> : MappingPaginatedList<TResult>, IMappingPaginatedList_withURL<TResult>
    {
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public MappingPaginatedList_withURL(List<TResult> data, int currentPage, int pageSize): base(data, currentPage, pageSize)
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
            Uri prevpage = this.CurrentPage - 1 >= 1 && this.CurrentPage <= this.PageCount ? urisvc.GetPageUri(
                                    new PaginationFilter(this.CurrentPage - 1, this.PageSize), route) : null;
            Uri nextpage = this.CurrentPage >= 1 && this.CurrentPage < this.PageCount ? urisvc.GetPageUri(
                                    new PaginationFilter(this.CurrentPage + 1, this.PageSize), route) : null;
        }

        /// <summary>
        /// Call this static method to create a paginated list from an IQueryable instance.
        /// It will do the skip/take for the appropriate page of data to return.
        /// And, will populate the page count and other metrics.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mapper">IMapper instance needed here</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="urisvc"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        static public new async Task<MappingPaginatedList_withURL<TResult>> CreateAsync<TSource>(IQueryable<TSource> source,
                                                                                             IMapper mapper,
                                                                                             int pageNumber,
                                                                                             int pageSize,
                                                                                             IUriService urisvc,
                                                                                             string route)
        {

            OGA.SharedKernel.QueryHelpers.PaginationFilter pgfilter = new PaginationFilter(pageNumber, pageSize);
            return await CreateAsync(source, mapper, pgfilter, urisvc, route);
        }
        /// <summary>
        /// Call this static method to create a paginated list from an IQueryable instance.
        /// It will do the skip/take for the appropriate page of data to return.
        /// And, will populate the page count and other metrics.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mapper">IMapper instance needed here</param>
        /// <param name="pgfilter"></param>
        /// <param name="urisvc"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        static public new async Task<MappingPaginatedList_withURL<TResult>> CreateAsync<TSource>(IQueryable<TSource> source,
                                                                                             IMapper mapper,
                                                                                             OGA.SharedKernel.QueryHelpers.PaginationFilter pgfilter,
                                                                                             IUriService urisvc,
                                                                                             string route)
        {
            // Check that the page index is valid...
            if (pgfilter.pageSize < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("pageSize invalid. Must be positive.");

            // Check that the page index is valid...
            if (pgfilter.pageNumber < 1)
                throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Must be positive.");

            // Count how many result entries in the query...
            var queryresult_count = await source.CountAsync();

            // Determine how many pages required to display the query results...
            var pagecount = (int)Math.Ceiling((double)queryresult_count / pgfilter.pageSize);

            //// See if the page index is out of range, high...
            //if(pageIndex > pagecount)
            //    throw new OGA_SharedKernel.Exceptions.BusinessRuleBrokenException("PageIndex invalid. Out of range, high.");

            // Calculate urls for the page...
            Uri firstpage = urisvc.GetPageUri(new PaginationFilter(1, pgfilter.pageSize), route);
            Uri lastpage = urisvc.GetPageUri(new PaginationFilter(pagecount, pgfilter.pageSize), route);
            Uri prevpage = pgfilter.pageNumber - 1 >= 1 && pgfilter.pageNumber <= pagecount ? urisvc.GetPageUri(new PaginationFilter(pgfilter.pageNumber - 1, pgfilter.pageSize), route) : null;
            Uri nextpage = pgfilter.pageNumber >= 1 && pgfilter.pageNumber < pagecount ? urisvc.GetPageUri(new PaginationFilter(pgfilter.pageNumber + 1, pgfilter.pageSize), route) : null;

            // Get the desired page of items from the query results...
            var items = await source.Skip((pgfilter.pageNumber - 1) * pgfilter.pageSize)
                                .Take(pgfilter.pageSize)
                                .Select(ufc => mapper.Map<TResult>(ufc))
                                .ToListAsync();

            // Return the paginated list to the caller...
            return new MappingPaginatedList_withURL<TResult>(items, pgfilter.pageNumber, pgfilter.pageSize)
            {
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
