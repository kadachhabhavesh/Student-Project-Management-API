using System;
using System.Collections.Generic;

namespace StudentProjectManagementAPI.Models;

public partial class File
{
    public int FileId { get; set; }

    public int? ProjectId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public int? FileSize { get; set; }

    public DateTime? UploadedAt { get; set; }

    public int? UploadedBy { get; set; }

    public virtual Project? Project { get; set; }

    public virtual User? UploadedByNavigation { get; set; }
}
