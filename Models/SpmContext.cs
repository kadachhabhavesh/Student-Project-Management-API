using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentProjectManagementAPI.Models;

public partial class SpmContext : DbContext
{
    public SpmContext()
    {
    }

    public SpmContext(DbContextOptions<SpmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentEvaluation> StudentEvaluations { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TeamMember> TeamMembers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=BHAVESH;database=SPM;trusted_connection=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.EvaluationId).HasName("PK__Evaluati__36AE68D3747ACC6F");

            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PK__Faculty__306F636E32047697");

            entity.ToTable("Faculty");

            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Faculties)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Faculty__UserID__534D60F1");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__Files__6F0F989F18798A26");

            entity.Property(e => e.FileId).HasColumnName("FileID");
            entity.Property(e => e.FileName).HasMaxLength(200);
            entity.Property(e => e.FilePath).HasMaxLength(200);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.UploadedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Project).WithMany(p => p.Files)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Files__ProjectID__1332DBDC");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Files)
                .HasForeignKey(d => d.UploadedBy)
                .HasConstraintName("FK__Files__UploadedB__14270015");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABED05F68828E");

            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.MentorId).HasColumnName("MentorID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Mentor).WithMany(p => p.Projects)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK__Projects__Mentor__571DF1D5");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A79F2FCA098");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.EnrollmentNo).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Students__UserID__5070F446");
        });

        modelBuilder.Entity<StudentEvaluation>(entity =>
        {
            entity.HasKey(e => e.StudentEvaluationsId).HasName("PK__StudentE__9989E6090A079B87");

            entity.Property(e => e.StudentEvaluationsId).HasColumnName("StudentEvaluationsID");
            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Faculty).WithMany(p => p.StudentEvaluations)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FK__StudentEv__Facul__0A9D95DB");

            entity.HasOne(d => d.Project).WithMany(p => p.StudentEvaluations)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__StudentEv__Proje__09A971A2");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentEvaluations)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__StudentEv__Stude__0B91BA14");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949D1BB507338");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK__Tasks__AssignedT__7B5B524B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaskCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Tasks__CreatedBy__7C4F7684");

            entity.HasOne(d => d.LastModifiedByNavigation).WithMany(p => p.TaskLastModifiedByNavigations)
                .HasForeignKey(d => d.LastModifiedBy)
                .HasConstraintName("FK__Tasks__LastModif__7D439ABD");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__Tasks__LastModif__7A672E12");
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(e => e.TeamMemberId).HasName("PK__TeamMemb__C7C09285A071B0D2");

            entity.Property(e => e.TeamMemberId).HasColumnName("TeamMemberID");
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Project).WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK__TeamMembe__Proje__59FA5E80");

            entity.HasOne(d => d.Student).WithMany(p => p.TeamMembers)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__TeamMembe__Stude__5AEE82B9");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC16453BCA");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053487B8131F").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(300);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
