using ClaimsPlugin.Shared.Foundation.Features.Auditing.Dtos;
using ClaimsPlugin.Shared.Foundation.Features.Auditing.Entities;
using ClaimsPlugin.Shared.Foundation.Features.Auditing.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace ClaimsPlugin.Shared.Foundation.Features.Auditing.Services;
public class AuditService<TDbContext> : IAuditService where TDbContext : DbContext, IAuditableDbContext
{
    private readonly TDbContext _context;
    public AuditService(TDbContext context)
    {
        _context = context;
    }
    public async Task<List<AuditDto>> GetUserTrailsAsync(string userId)
    {
        List<Trail> trails = await _context.AuditTrails
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();
        return trails.Adapt<List<AuditDto>>();
    }
}
