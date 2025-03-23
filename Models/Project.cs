using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StudentProjectManagementAPI.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Status { get; set; }

    public int? MentorId { get; set; }

    public virtual Faculty? Mentor { get; set; }

    [JsonIgnore]
    public virtual ICollection<File> Files { get; set; } = new List<File>();

    [JsonIgnore]
    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    [JsonIgnore]
    public virtual ICollection<StudentEvaluation> StudentEvaluations { get; set; } = new List<StudentEvaluation>();

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [JsonIgnore]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}
