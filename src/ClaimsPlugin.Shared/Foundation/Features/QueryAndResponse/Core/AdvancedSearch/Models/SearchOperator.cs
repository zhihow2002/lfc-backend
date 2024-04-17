using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Features.QueryAndResponse.Core.AdvancedSearch.Models
{
    public static class SearchOperator
    {
        public static string Eq => "eq";
        public static string Neq => "neq";
        public static string Lt => "lt";
        public static string Lte => "lte";
        public static string Gt => "gt";
        public static string Gte => "gte";
        public static string StartsWith => "sw";
        public static string EndsWith => "ew";
        public static string Contains => "c";
    }
}
