using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace StudentProjectManagementAPI.Models;

public partial class User
{
    public int? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Role { get; set; }

    public string? Department { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    [JsonIgnore]
    public virtual ICollection<File> Files { get; set; } = new List<File>();

    [JsonIgnore]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [JsonIgnore]
    public virtual ICollection<Task> TaskCreatedByNavigations { get; set; } = new List<Task>();

    [JsonIgnore]
    public virtual ICollection<Task> TaskLastModifiedByNavigations { get; set; } = new List<Task>();
}
