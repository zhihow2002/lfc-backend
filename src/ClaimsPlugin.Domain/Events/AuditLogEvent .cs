using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Domain.Events
{
    public class AuditLogEvent : IAuditLogEvent
    {
        public string EventType => this.Data.GetType().Name;
        public string ReferenceId { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}