using System;
using System.Collections.Generic;

namespace StudentProjectManagementAPI.Models;

public partial class TeamMember
{
    public int TeamMemberId { get; set; }

    public int? ProjectId { get; set; }

    public int? StudentId { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Student? Student { get; set; }
}
