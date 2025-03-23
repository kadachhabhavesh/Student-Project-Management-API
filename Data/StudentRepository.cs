using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class StudentRepository
    {
        public readonly SpmContext _context;
        #region StudentRepository
        public StudentRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region StudentList
        public async Task<List<Student>> StudentList()
        {
            var studentList = await _context.Students.Include(s => s.User).ToListAsync();
            return studentList;
        }
        #endregion

        #region GetStudentById
        public async Task<Student> GetStudentById(int studentId)
        {
            Student student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
            {
                return null;
            }
            return student;
        }
        #endregion

        #region AddStudent
        public async Task<bool> AddStudent(Student student)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (student.User != null)
                {
                    student.User.CreatedAt = DateTime.Now;
                    student.User.UpdatedAt = DateTime.Now;
                    student.User.UserId = null;
                    _context.Users.Add(student.User);
                    await _context.SaveChangesAsync();
                }

                student.UserId = student.User.UserId;
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error add student: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region UpdateStudent
        public async Task<bool> UpdateStudent(Student student)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (student.User != null)
                {
                    student.User.UpdatedAt = DateTime.Now;
                    _context.Users.Update(student.User);
                    await _context.SaveChangesAsync();
                }
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }
        #endregion

        #region DeleteStudent
        public async Task<bool> DeleteStudent(int studentId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentId == studentId);
                if (student == null) return false;

                var user = student.User;

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region StudentProfile
        public async Task<Object> StudentProfile(int studentId)
        {

            Student student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.StudentId == studentId);
           
            if (await _context.TeamMembers
                .Where(tm => tm.StudentId == studentId)
                .Select(tm => tm.ProjectId)
                .Distinct()
                .CountAsync() > 0)
            {
                var projects = await _context.TeamMembers
                .Include(tm => tm.Project).Where(p => p.StudentId == studentId)
                .Include(tm => tm.Project.Mentor)
                .Include(tm => tm.Project.Mentor.User)
                .Select(p => new
                {
                    projectId = p.Project.ProjectId,
                    title = p.Project.Title,
                    Description = p.Project.Description,
                    StartDate = p.Project.StartDate,
                    EndDate = p.Project.EndDate,
                    Status = p.Project.Status,
                    Mentor = p.Project.Mentor,
                    Tasks = _context.Tasks
                            .Where(t => t.ProjectId == p.Project.ProjectId && t.AssignedTo == student.StudentId)
                            .Select(t => new
                            {
                                TaskId = t.TaskId,
                                Title = t.Title,
                                Description = t.Description,
                                Deadline = t.Deadline,
                                Priority = t.Priority,
                                Status = t.Status
                            })
                            .ToList(),
                    members = _context.TeamMembers
                        .Where(tm1 => tm1.Project.ProjectId == p.Project.ProjectId)
                        .Include(tm2 => tm2.Student.User)
                        .Select(s => new {
                            TeamMembers = s.TeamMemberId,
                            studentId = s.Student.StudentId,
                            Name = s.Student.User.FirstName + " " + s.Student.User.LastName,
                            EnrollmentNo = s.Student.EnrollmentNo,
                            Email = s.Student.User.Email
                        })
                        .ToList(),
                    Evaluations = _context.Evaluations
                        .Where(e => e.ProjectId == p.Project.ProjectId)
                        .Include(e => e.Faculty.User)
                        .Select(e => new {
                            EvaluatedBy = e.Faculty.User.FirstName + " " + e.Faculty.User.LastName,
                            Feedback = e.Feedback,
                            Score = e.Score
                        })
                        .ToList(),
                    StudentEvaluations = _context.StudentEvaluations
                        .Where(e => e.ProjectId == p.Project.ProjectId && e.Student.StudentId == student.StudentId)
                        .Include(e => e.Student.User)
                        .Select(e => new {
                            EvaluatedBy = e.Student.User.FirstName + " " + e.Student.User.LastName,
                            Feedback = e.Feedback,
                            Score = e.Score
                        })
                        .ToList()
                }).ToListAsync();
                return new
                {
                    student,
                    projects
                };
            }

            return new
            {
                student
            };
        }
        #endregion
    }
}