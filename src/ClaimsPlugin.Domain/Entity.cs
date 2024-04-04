using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Domain
{
    public class Entity
    {
         public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; private set; } = DateTimeOffset.UtcNow;
        public string CreatedBy { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public string UpdatedBy { get; private set; }

        public void SetCreator(string creator)
        {
            CreatedBy = creator;
            CreatedOn = DateTimeOffset.UtcNow;
        }

        public void SetUpdater(string updater)
        {
            UpdatedBy = updater;
            UpdatedOn = DateTimeOffset.UtcNow;
        }
    }
}