using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.AdvancedSearch.Models
{
    public class Search
    {
        public IEnumerable<Search>? Groups { get; set; } = default!;
        public string? Field { get; set; } = default!;
        public object? Value { get; set; } = default!;
        public string? Logic { get; set; } = default!;
        public string Operator { get; set; } = SearchOperator.Eq;
    }
}
