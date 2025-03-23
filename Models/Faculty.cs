using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StudentProjectManagementAPI.Models;

public partial class Faculty
{
    public int? UserId { get; set; }

    public int FacultyId { get; set; }

    public string? Designation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    [JsonIgnore]
    public virtual ICollection<StudentEvaluation> StudentEvaluations { get; set; } = new List<StudentEvaluation>();

    public virtual User? User { get; set; }
}
