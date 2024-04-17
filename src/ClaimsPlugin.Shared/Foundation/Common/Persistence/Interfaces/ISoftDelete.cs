using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces
{
    public interface ISoftDelete
    {
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public Guid? DeletedById { get; set; }
        public string? DeletedByType { get; set; }
    }
}
