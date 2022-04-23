using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[Index(nameof(Storage), nameof(Path), nameof(Version), IsUnique = true)]
public class ObjectStorageFile
{
    [Key]
    public Int64 Id { get; set; }
    
    public string BackupLocation { get; set; } = "";
    public string Hash { get; set; } = "";
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public string Storage { get; set; } = "";
    
    public DateTime CreatedAt { get; set; }
    public DateTime? SyncedAt { get; set; }
    public int Version { get; set; }
}
