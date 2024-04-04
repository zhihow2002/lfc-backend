using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Domain.Events
{
    public interface IAuditLogEvent : IEvent
    {
         public string EventType { get; }
        public string ReferenceId { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}