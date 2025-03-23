using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StudentProjectManagementAPI.Models;

public partial class Student
{
    public int? UserId { get; set; }

    public int StudentId { get; set; }

    public string? EnrollmentNo { get; set; }

    [JsonIgnore]
    public virtual ICollection<StudentEvaluation> StudentEvaluations { get; set; } = new List<StudentEvaluation>();

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [JsonIgnore]
    public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

    public virtual User? User { get; set; }
}
