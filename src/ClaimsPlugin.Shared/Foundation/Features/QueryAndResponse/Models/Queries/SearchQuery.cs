using ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.AdvancedSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Models.Queries
{
    public abstract class SearchQuery
    {
        /// <summary>
        ///     Column Wise Search is Supported.
        /// </summary>
        public Search? AdvancedSearch { get; set; } = default!;

        /// <summary>
        ///     Indicates how many pages to be included in this query.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        ///     Indicates how many records to be included in this query.
        /// </summary>
        public int PageSize { get; set; } = int.MaxValue;

        /// <summary>
        ///     Order by name of the column, default to ascending order. Use "ColumnName ASC" or "ColumnName DESC" for different
        ///     ordering.
        /// </summary>
        public string[]? OrderBy { get; set; } = default!;
    }
}
