using System.ComponentModel;

namespace MyVdsFactory.Domain.Enums;

public enum EntityStatus
{
    [Description("Aktif")]
    Active = 1,
    [Description("Pasif")]
    Passive,
    [Description("Arşivlenmiş")]
    Archived,
    [Description("Gizli")]
    Hidden
}