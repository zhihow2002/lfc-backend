using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClaimsPlugin.Shared.Foundation.Features.Auditing.Entities;
using ClaimsPlugin.Shared.Foundation.Features.Auditing.Enums;
using ClaimsPlugin.Shared.Foundation.Features.Serializer.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ClaimsPlugin.Shared.Foundation.Features.Auditing.Models;

public class AuditTrail
{
    private readonly ISerializerService _serializer;

    public AuditTrail(EntityEntry entry, ISerializerService serializer)
    {
        Entry = entry;
        _serializer = serializer;
    }

    public EntityEntry Entry { get; }
    public Dictionary<string, object?> KeyValues { get; } = new();
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<PropertyEntry> TemporaryProperties { get; } = new();
    public List<string> ChangedColumns { get; } = new();
    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;
    public string UserId { get; set; } = default!;
    public string? TableName { get; set; } = default!;
    public TrailType TrailType { get; set; }

    public Trail ToAuditTrail()
    {
        return new Trail
        {
            UserId = UserId,
            Type = TrailType.ToString(),
            TableName = TableName,
            DateTime = DateTime.Now,
            PrimaryKey = _serializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : _serializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : _serializer.Serialize(NewValues),
            AffectedColumns =
                ChangedColumns.Count == 0 ? null : _serializer.Serialize(ChangedColumns)
        };
    }
}
