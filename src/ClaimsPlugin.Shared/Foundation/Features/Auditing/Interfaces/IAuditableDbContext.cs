using ClaimsPlugin.Shared.Foundation.Features.Auditing.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPlugin.Shared.Foundation.Features.Auditing.Interfaces;

public interface IAuditableDbContext
{
    public DbSet<Trail> AuditTrails { get; }
}