using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Models;

public abstract class BaseDatabaseSeeder : IDatabaseSeeder
{
    public abstract Task SeedDatabaseAsync(CancellationToken cancellationToken);
}
