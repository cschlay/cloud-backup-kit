using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

/// <summary>
/// Keep track of deleted files, the deletion of these should be delayed.
/// </summary>
public class ObjectStorageDeleteLog
{
    [Key]   
    public Int64 Id { get; set; }
    
    [ForeignKey("FileId")]
    public ObjectStorageFile? File { get; set; }
    public Int64 FileId { get; set; }
    
    public DateTime DeletedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
}
