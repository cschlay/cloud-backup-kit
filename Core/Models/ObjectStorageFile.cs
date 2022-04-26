using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Models;

[Index(nameof(Storage), nameof(Path), nameof(Version), IsUnique = true)]
public class ObjectStorageFile
{
    [Key]
    public Int64 Id { get; set; }
    
    public string BackupLocation { get; set; } = "";
    public string ContentType { get; set; } = "";
    public string Hash { get; set; } = "";
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public string Storage { get; set; } = "";

    public string SignedDownloadUrl { get; set; } = "";
    
    public DateTime CreatedAt { get; set; }
    public DateTime? SyncedAt { get; set; }

    [Column(TypeName = "varchar(10)")]
    public SyncStatusEnum Status { get; set; }
    public int Version { get; set; }
}

public class ObjectStorageFileConfiguration : IEntityTypeConfiguration<ObjectStorageFile>
{
    public void Configure(EntityTypeBuilder<ObjectStorageFile> builder)
    {
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("now()");
    }
}
