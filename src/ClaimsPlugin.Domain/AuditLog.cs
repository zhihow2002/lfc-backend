using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Domain
{
    public class AuditLog
    {
        public int EventId { get; set; }
        public DateTimeOffset InsertedDate { get; set; }
        public DateTimeOffset? LastUpdatedDate { get; set; }
        public string JsonData { get; set; }
        public string EventType { get; set; }
        public string ReferenceId { get; set; }
        public string IpAddress { get; set; }
    }
}