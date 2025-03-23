using System;
using System.Collections.Generic;

namespace StudentProjectManagementAPI.Models;

public partial class StudentEvaluation
{
    public int StudentEvaluationsId { get; set; }

    public int? ProjectId { get; set; }

    public int? FacultyId { get; set; }

    public double? Score { get; set; }

    public string? Feedback { get; set; }

    public int? StudentId { get; set; }

    public DateOnly? EvaluatedAt { get; set; }

    public virtual Faculty? Faculty { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Student? Student { get; set; }
}
