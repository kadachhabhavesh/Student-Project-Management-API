using System;
using System.Collections.Generic;

namespace StudentProjectManagementAPI.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public int? ProjectId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Priority { get; set; }

    public int? AssignedTo { get; set; }

    public DateOnly? Deadline { get; set; }

    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public int? LastModifiedBy { get; set; }

    public virtual Student? AssignedToNavigation { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? LastModifiedByNavigation { get; set; }

    public virtual Project? Project { get; set; }
}
