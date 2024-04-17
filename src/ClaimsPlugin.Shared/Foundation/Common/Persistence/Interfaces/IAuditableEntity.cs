using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces
{
    public interface IAuditableEntity
    {
        public DateTime CreatedOn { get; }
        public string CreatedBy { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByType { get; set; }
        public string? LastModifiedBy { get; set; }
        public Guid LastModifiedById { get; set; }
        public string LastModifiedByType { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
